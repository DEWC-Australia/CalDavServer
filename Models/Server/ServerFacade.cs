using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem.ACL;
using CalDav.Models.FileSystem.Folder;
using CalDav.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CalDav.Models.Server
{
    public class ServerFacade : ICalDavServer
    {
        private Stream stream { get; set; }
        public ServerFacade(CALDAVContext db)
        {
            mDb = db;
            // Will throw an exception if many or no active server descriptions are found
            Server = mDb.CalDavServer
                                .Include(a => a.ContextPathNavigation)
                                .Where(a => a.Active).Single();
            
        }

        private CALDAVContext mDb { get; set; }
        protected Stream Stream { get; set; }
        protected CalDavServer Server { get; set; }
        public UserProfile GetCurrentUser(HttpRequest httpRequest)
        {
            string authHeader = httpRequest.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                // Split username and password
                var userName = decodedUsernamePassword.Split(':', 2)[0];

                return  mDb.UserProfile
                            .Include(a => a.AclFolderNavigation)
                            .Include(a => a.CalendarHomeSetNavigation)
                            .Where(a => a.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
            }

            return null;
        }

        public IPrincipalItem GetPrincipalItem(UserProfile currentUser)
        {
            return new PrincipalItem(currentUser, mDb);
        }

        protected string CleanPath(string path)
        {
            if (path.First().Equals("/"))
                path = path.Remove(0, 1);

            return path.Replace("/", "\\");
        }

        public AbstractFolder GetFileSystemInfo(string path, int depth, IPrincipalItem principalItem)
        {
            // the request is for a file
            string file = null;
            string fileextension = ".ics";
  
            //00000000-0000-0000-0000-000000000000
            Guid fileGuid = new Guid();
            if (path.EndsWith(fileextension))
            {
                var temp = path.Split("/");

                file = temp.Last();
                path = path.Remove(path.Length - file.Length, file.Length);

                if (!Guid.TryParse(file.Remove(file.Length - fileextension.Length, fileextension.Length), out fileGuid))
                {
                    return null;
                }
                
            }

            if (!path.Contains("well-known") && path.Last() != '/')
                path += "/";

            var userFolderAccess = principalItem.TestAccess(path, Server);

            // check that a folder was found and that the user has read access
            if (userFolderAccess == null || userFolderAccess.Folder == null || !userFolderAccess.Read)
                return null;

            var fileinfo = userFolderAccess.Folder;

            // if the url has a file but is not a path to a calendar then the url is not valid
            if (fileinfo.FolderType != (int)FileSystem.FolderType.CalendarFolder && file != null)
                return null;


            // we are now at a safe place to physically ensure the folder exists
            var dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"{CalDavSettings.SERVERFILEPATH}\\{CleanPath(path)}");

            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            if (fileinfo.FolderType == (int)FileSystem.FolderType.WellKnown)
                return new WellKnownFolder(userFolderAccess, fileinfo, Server);

            AbstractFolder result = null;

            switch (fileinfo.FolderType)
            {
                case (int)FileSystem.FolderType.CalendarFolder:
                    result = new CalendarRepository(mDb, userFolderAccess, fileinfo, Server, fileGuid);
                    break;
                case (int)FileSystem.FolderType.CalendarHomeset:
                    result = new CalendarHomeSet(userFolderAccess, fileinfo, Server);
                    break;
                case (int)FileSystem.FolderType.ContextPath:
                    result = new ContextPath(principalItem, userFolderAccess, fileinfo, Server);
                    break;
                case (int)FileSystem.FolderType.AclFolder:
                    result = new PrincipalFolder(principalItem, userFolderAccess, fileinfo, Server);
                    break;
                default:
                    return null;
            }

            // Add Children
            if(depth > 0)
            {
                depth--;
                var childPaths = mDb.FolderInfo.Where(a => a.ParentFolderId.Equals(fileinfo.FolderId)).Select(a => a.Path).ToList();
                result.ChildFolders = new List<AbstractFolder>();
                foreach (var childpath in childPaths)
                {
                    
                    var childFileInfo = GetFileSystemInfo(childpath, depth, principalItem);
                    if(childFileInfo != null)
                        result.ChildFolders.Add(childFileInfo);
                }
            }

            return result;
        }

        public CalDavRequest GetRequestType(string method, CALDAVContext mDb, CalDavRequestData requestData, AbstractFolder folder)
        {
            return CalDavRequestFactory.GetRequest(method, mDb, requestData, folder);
        }

        public CalDavRequestData BuildRequestData(HttpRequest httpRequest, IPrincipalItem currentPrincipal)
        {
            return new CalDavRequestData
            {
                Depth = SolveDepth(httpRequest),
                XmlBody = GetRequestXml(httpRequest),
                CurrentPrincipal = currentPrincipal,
                CurrentCalendar = GetRequestCalendar(httpRequest)
            };
        }

        public Calendar GetRequestCalendar(HttpRequest httpRequest)
        {
            if (!(httpRequest.ContentType ?? string.Empty).ToLower().Contains("calendar") || httpRequest.ContentLength == 0)
            {
                return null;
            }
            var serializer = new Serializer();

            using (var str = (Stream ?? httpRequest.Body))
            {
                var ct = httpRequest.Headers.Where(a => a.Key == CalDavSettings.HttpHeader.CONTENTTYPE).FirstOrDefault();
                System.Text.Encoding encoding = System.Text.Encoding.Default;
                if (ct.Value.Contains("utf-8"))
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                var ical = serializer.Deserialize<Calendar>(str, encoding);

                return ical;
            }
        }

        protected int SolveDepth(HttpRequest httpRequest)
        {
            //https://stackoverflow.com/questions/31284615/meaning-of-depth-header-in-webdav-propfind-method
            var depth = 0;

            if (httpRequest.Headers.ContainsKey("Depth") && httpRequest.Headers["Depth"].Count() == 1)
            {
                var depthString = httpRequest.Headers["Depth"].First();

                if (!int.TryParse(depthString, out depth))
                    depth = 0;

                // not supporting recurrsive depth yet
                if (depth > 1)
                    depth = 1;
            }

            return depth;
        }

        protected XDocument GetRequestXml(HttpRequest httpRequest)
        {
            if (!(httpRequest.ContentType ?? string.Empty).ToLower().Contains("xml") || httpRequest.ContentLength == 0)
            {
                return null;
            } 

            using (var _stream = (stream ?? httpRequest.Body))
            {
                return XDocument.Load(_stream);
            }
        }
        /*
        protected string formatTextXml(string raw)
        {
           // raw.Split(' ');
           // raw.Split('\n');
        }
        */
    }
}
