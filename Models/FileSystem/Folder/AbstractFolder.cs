using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem.ACL;
using CalDav.Models.FileSystem.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalDav.Models.FileSystem.Folder
{
    public abstract class AbstractFolder : IFileSystemInfo, IFileSystemAccess
    {
        public CalDavServer Server { get; protected set; }
        public AbstractFolder(UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server)
        {
            Server = server;
            Load(userAccess, folderInfo, server);
        }

        protected string CleanPath(string path)
        {
            if (path.First().Equals("/"))
                path = path.Remove(0, 1);

            return path.Replace("/", "\\");
        }

        public List<AbstractFolder> ChildFolders { get; set; }

        // options are a combination of user permissions and system settings
        public List<IOption> Options { get; protected set; }

        public string AllowOptions { get; protected set; }
        public string PublicOptions { get; protected set; }

        public string GetOptions
        {
            get
            {
                StringBuilder options = new StringBuilder();
                foreach (var name in Options.OrderBy(a => a.Order).Select(a => a.Name).ToList())
                { options.Append(name + ", "); }

                options.Remove((options.Length - 2), 2);

                return options.ToString();
            }
        }

        public Guid FolderID { get; protected set; }
        // folder info
        public DateTime Created { get; protected set; }
        public DateTime Modified { get; protected set; }
        public string Name { get; protected set; }
        public string Path { get; protected set; }
        public FolderType Type { get; protected set; }
        public bool Paging { get; protected set; }

        // UserFolderAccess Details
        public bool AccessControl { get; protected set; }
        public bool CalendarAccess { get; protected set; }
        public bool CalendarServerSharing { get; protected set; }
        public bool Owner { get; protected set; }
        public bool Read { get; protected set; }
        public bool Write { get; protected set; }

       // common Functions
        protected void Load(UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server)
        {
            Options = new List<IOption>();
            LoadAccess(userAccess);
            LoadInfo(folderInfo);
            LoadUserOptions();
            LoadSystemOptions(server);
            AllowOptions = server.AllowOptions;
            PublicOptions = server.PublicOptions;
        }

        protected void LoadAccess(UserFolderAccess userAccess)
        {
            AccessControl = userAccess.AccessControl;
            CalendarAccess = userAccess.CalendarAccess;
            CalendarServerSharing = userAccess.CalendarServerSharing;
            Owner = userAccess.Owner;
            Read = userAccess.Read;
            Write = userAccess.Write;
        }

        protected void LoadInfo(FolderInfo folderInfo)
        {
            FolderID = folderInfo.FolderId;
            Created = folderInfo.Created;
            Modified = folderInfo.Modified;
            Name = folderInfo.Name;
            Path = folderInfo.Path;
            Type = GetFolderType(folderInfo.FolderType);
            Paging = folderInfo.Paging;
        }

        protected void LoadUserOptions()
        {
            if (AccessControl)
                Options.Add(new AccessControl());
            if (CalendarAccess)
                Options.Add(new CalendarAccess());
            if (CalendarServerSharing)
                Options.Add(new CalendarServerSharing());

        }

        protected void LoadSystemOptions(CalDavServer server)
        {
            if(server.DavLevel1)
                Options.Add(new DAVLevel1());
            if (server.DavLevel2)
                Options.Add(new DAVLevel2());
            if (server.DavLevel3)
                Options.Add(new DAVLevel3());
            if (server.DavLevel3)
                Options.Add(new Paging());
        
        }

        protected FolderType GetFolderType(int folderType)
        {
            switch (folderType)
            {
                case (int)FolderType.WellKnown:
                    return FolderType.WellKnown;
                case (int)FolderType.ContextPath:
                    return FolderType.ContextPath;
                case (int)FolderType.CalendarHomeset:
                    return FolderType.CalendarHomeset;
                case (int)FolderType.CalendarFolder:
                    return FolderType.CalendarFolder;
                case (int)FolderType.AclFolder:
                    return FolderType.AclFolder;

                default:
                    throw new NotImplementedException($"Internal Server Error - Folder Type ({folderType}) not implemented");
            }

        }
    }
}
