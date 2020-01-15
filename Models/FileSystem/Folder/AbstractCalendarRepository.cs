using CalDav.CalendarObjectRepository;
using CalDav.Data.CALDAV;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CalDav.Models.FileSystem.Folder
{
    public abstract class AbstractCalendarRepository : AbstractFolder, ICalendarRepository
    {
        private CALDAVContext mDb { get; set; }

        public Guid File { get; protected set; }

        protected string WebServerRootDirectory { get; set; }
        public AbstractCalendarRepository(CALDAVContext db, UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server, Guid file) : base(userAccess, folderInfo, server)
        {
           
            File = file;
            mDb = db;
            LoadCalendarInfo(folderInfo.CalendarFolderInfo);
            WebServerRootDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), CalDavSettings.SERVERFILEPATH);
            Directory.CreateDirectory(WebServerRootDirectory);
        }

        protected void LoadCalendarInfo(CalendarFolderInfo calendarInfo)
        {
#warning Need to add content length 
            this.ContentLength = 0;
            this.ContentType = calendarInfo.ContentType;

            this.Ctag = Convert.ToBase64String(calendarInfo.Ctag);
            
            this.CalendarColor = calendarInfo.CalendarColor;
            this.CalendarDescription = calendarInfo.CalendarDescription;

            var owners = this.mDb.UserFolderAccess.Include(a => a.User).Where(a => a.FolderId == this.FolderID && a.Owner).ToList();

			// a shared folder will have no owners
            this.SharedOwner = (owners.Count() == 0);

            OwnerNames = new List<string>();
            foreach (var currentOwner in owners)
            {
                OwnerNames.Add($"{this.Server.BaseAclPath}{currentOwner.User.UserName}");
            }

            this.AclPrincipalPropSet = calendarInfo.AclPrincipalPropSet;
            this.PrincipalMatch = calendarInfo.PrincipalMatch;
            this.PrincipalPropertySearch = calendarInfo.PrincipalPropertySearch;
            this.CalendarMultiGet = calendarInfo.CalendarMultiGet;
            this.CalendarQuery = calendarInfo.CalendarQuery;
        }

        public Calendar Calendar { get; protected set; }

        public long ContentLength { get; protected set; }
        public string ContentType { get; protected set; }
        public string Ctag { get; protected set; }
        public string CalendarColor { get; protected set; }
        public string CalendarDescription { get; protected set; }
        public List<string> OwnerNames { get; protected set; }
        // will look in useraccess for multiple owners
        public bool SharedOwner { get; protected set; }
        public bool AclPrincipalPropSet { get; protected set; }
        public bool PrincipalMatch { get; protected set; }
        public bool PrincipalPropertySearch { get; protected set; }
        public bool CalendarMultiGet { get; protected set; }
        public bool CalendarQuery { get; protected set; }




        public virtual ICalendarObject GetObjectByUID(Guid uid)
        {
            var filename = $"{WebServerRootDirectory}{this.CleanPath(this.Path)}{uid.ToString()}{CalDavSettings.fileExtension}";

            if (!System.IO.File.Exists(filename))
            {
                return null;
            }

            var serializer = new Serializer();

            using (var file = System.IO.File.OpenText(filename))
            {
                var ical = (serializer.Deserialize<Calendar>(file));

                return ical.Events.OfType<ICalendarObject>()
                    .Union(ical.ToDos)
                    .Union(ical.FreeBusy)
                    .Union(ical.JournalEntries)
                    .FirstOrDefault();
            }
        }

        public Calendar GetCalendarFileByUID(Guid uid)
        {
            var filename = $"{WebServerRootDirectory}{this.CleanPath(this.Path)}{uid.ToString()}{CalDavSettings.fileExtension}";

            if (!System.IO.File.Exists(filename))
            {
                return null;
            }

            var serializer = new Serializer();

            using (var file = System.IO.File.OpenText(filename))
            {
                return (serializer.Deserialize<Calendar>(file));

            }
        }

        public virtual void DeleteObject(Guid uid)
        {
            var obj = GetCalendarFileByUID(uid);

            if (obj == null)
            {
                return;
            }

            var filename = $"{WebServerRootDirectory}{this.CleanPath(this.Path)}{uid.ToString()}{CalDavSettings.fileExtension}";

            obj.Delete(mDb, this.FolderID, uid);

            if (!System.IO.File.Exists(filename))
            {
                return;
            }

            System.IO.File.Delete(filename);
        }
       
        public virtual IQueryable<ICalendarObject> GetObjects()
        {
            var directory = $"{WebServerRootDirectory}{this.CleanPath(this.Path)}";
            var files = System.IO.Directory.GetFiles(directory, "*.ics");
            var serializer = new Serializer();

            var many = files.Select(x => serializer.Deserialize<Calendar>(x)).ToList();
            var result = many.SelectMany(x => x.Items).ToList();
            return result.AsQueryable();
        }

        public virtual IQueryable<ICalendarObject> GetObjects(List<Guid> fileIds)
        {
            var directory = $"{WebServerRootDirectory}{this.CleanPath(this.Path)}";

            fileIds.Select(a => $"{directory}{a.ToString()}.ics").ToArray();

            var files = System.IO.Directory.GetFiles(directory, "*.ics");

            var serializer = new Serializer();
            var many = files.Select(x => serializer.Deserialize<Calendar>(x)).ToList();
            var result = many.SelectMany(x => x.Items).ToList();
            return result.AsQueryable();
        }

        public virtual IQueryable<ICalendarObject> GetObjectsByFilter(Filter filter)
        {
            List<Guid> results = new List<Guid>();

            var calAction = filter.Filters.Where(a => a.Name == "VCALENDAR").FirstOrDefault();

            foreach(var eventsFilter in calAction.Filters.Where(a => a.Name == "VEVENT").ToList())
            {
                // time range
                if (eventsFilter.TimeRange != null && eventsFilter.TimeRange.End == null)
                {
                    results.AddRange( mDb.CalendarEvent.Where(a => a.FolderId == this.FolderID && a.Start >= eventsFilter.TimeRange.Start)
                        .Select(a => a.CalendarFileId).ToList());
                }else if(eventsFilter.TimeRange != null)
                {
                    results.AddRange(mDb.CalendarEvent.Where(a => a.FolderId == this.FolderID && (a.Start >= eventsFilter.TimeRange.Start && a.End <= eventsFilter.TimeRange.End))
                        .Select(a => a.CalendarFileId).ToList());
                }

                // Paramter Filter
                // Property Filter
                // compare filter
            }

            foreach (var toDoFilter in calAction.Filters.Where(a => a.Name == "VTODO").ToList())
            {
                // time range
                if (toDoFilter.TimeRange != null && toDoFilter.TimeRange.End == null)
                {
                    results.AddRange(mDb.CalendarEvent.Where(a => a.FolderId == this.FolderID && a.Start >= toDoFilter.TimeRange.Start)
                        .Select(a => a.CalendarFileId).ToList());
                }
                else if (toDoFilter.TimeRange != null)
                {
                    results.AddRange(mDb.CalendarEvent.Where(a => a.FolderId == this.FolderID && (a.Start >= toDoFilter.TimeRange.Start && a.End <= toDoFilter.TimeRange.End))
                        .Select(a => a.CalendarFileId).ToList());
                }

                // Paramter Filter
                // Property Filter
                // compare filter
            }

            return GetObjects(results).AsQueryable();
        }


        public virtual System.Net.HttpStatusCode Save(Calendar ical, string UID)
        {
            var filename = $"{WebServerRootDirectory}{this.CleanPath(this.Path)}{UID}{CalDavSettings.fileExtension}";
 
            System.Net.HttpStatusCode result = System.Net.HttpStatusCode.Created;

            if (System.IO.File.Exists(filename))
            {
                result = System.Net.HttpStatusCode.NoContent;
            }

            ical.Save(mDb, this.FolderID, Guid.Parse(UID));

            var serializer = new Serializer();

            using (var file = System.IO.File.Open(filename, System.IO.FileMode.Create))
            {
                serializer.Serialize(file, ical);
            }

            return result;
        }
        // not looking to support calendar creation at this point
        public ICalendarInfo CreateCalendar(string id)
        {
            throw new NotImplementedException();
        }

        
    }
}
