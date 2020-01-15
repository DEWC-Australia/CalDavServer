using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CalDav.Data.CALDAV;
using Microsoft.EntityFrameworkCore;

namespace CalDav.CalendarObjectRepository
{
    public class Calendar : ObjectBase, ISerializeToICAL
    {
        public Calendar()
        {
            Events = new List<Event>();
            TimeZones = new List<TimeZone>();
            ToDos = new List<ToDo>();
            JournalEntries = new List<JournalEntry>();
            FreeBusy = new List<FreeBusy>();
            Properties = new List<Tuple<string, string, System.Collections.Specialized.NameValueCollection>>();
        }
        public virtual Guid UID { get; set; }
		//CALDAV.CalendarFile
		[StringLength(50)]
        public virtual string Version { get; set; }
		[StringLength(100)]
        public virtual string ProdID { get; set; }
		[StringLength(100)]
		public string Scale { get; set; }
		public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }
        public virtual ICollection<TimeZone> TimeZones { get; set; }
        public virtual ICollection<JournalEntry> JournalEntries { get; set; }
        public virtual ICollection<FreeBusy> FreeBusy { get; set; }
        public ICollection<Tuple<string, string, System.Collections.Specialized.NameValueCollection>> Properties { get; set; }
		

        public virtual IQueryable<ICalendarObject> Items
        {
            get
            {
                return Events.OfType<ICalendarObject>()
                    .Union(ToDos).Union(JournalEntries).Union(FreeBusy)
                    .AsQueryable();
            }
        }

        public virtual void AddItem(ICalendarObject obj)
        {
            if (obj == null)
            {
                return;
            }

            if (obj is Event)
            {
                Events.Add((Event)obj);
            }
            else if (obj is ToDo)
            {
                ToDos.Add((ToDo)obj);
            }
            else if (obj is JournalEntry)
            {
                JournalEntries.Add((JournalEntry)obj);
            }
            else if (obj is FreeBusy)
            {
                FreeBusy.Add((FreeBusy)obj);
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public virtual void Deserialize(System.IO.TextReader rdr, Serializer serializer = null)
        {
            if (serializer == null) serializer = new Serializer();
            string name, value;
            var parameters = new System.Collections.Specialized.NameValueCollection();
            while (rdr.Property(out name, out value, parameters) && !string.IsNullOrEmpty(name))
            {
                switch (name.ToUpper())
                {
                    case "BEGIN":
                        switch (value)
                        {
                            case "VCALENDAR":
 
                                break;
                            case "VEVENT":
                                var e = serializer.GetService<Event>();
                                e.Calendar = this;
                                Events.Add(e);
                                e.Deserialize(rdr, serializer);
                                break;
                            case "VTIMEZONE":
                                var tz = serializer.GetService<TimeZone>();
                                tz.Calendar = this;
                                TimeZones.Add(tz);
                                tz.Deserialize(rdr, serializer);
                                break;
                            case "VTODO":
                                var td = serializer.GetService<ToDo>();
                                td.Calendar = this;
                                ToDos.Add(td);
                                td.Deserialize(rdr, serializer);
                                break;
                            case "VFREEBUSY":
                                var fb = serializer.GetService<FreeBusy>();
                                fb.Calendar = this;
                                FreeBusy.Add(fb);
                                fb.Deserialize(rdr, serializer);
                                break;
                            case "VJOURNAL":
                                var jn = serializer.GetService<JournalEntry>();
                                jn.Calendar = this;
                                JournalEntries.Add(jn);
                                jn.Deserialize(rdr, serializer);
                                break;
                        }
                        break;
                    case "CALSCALE": Scale = value; break;
                    case "VERSION": Version = value; break;
                    case "PRODID": ProdID = value; break;
                    case "END":
                        if (value == "VCALENDAR")
                            return;
                        break;
                    default:
                        Properties.Add(Tuple.Create(name, value, parameters));
                        break;
                }
            }
        }

        public virtual void Serialize(System.IO.TextWriter wrtr)
        {
            wrtr.BeginBlock("VCALENDAR");
            wrtr.Property("VERSION", Version ?? "2.0");
            wrtr.Property("PRODID", Common.PRODID);
            wrtr.Property("CALSCALE", Scale);

            if (Properties != null)
                foreach (var prop in Properties)
                    wrtr.Property(prop.Item1, prop.Item2, parameters: prop.Item3);

            foreach (var tz in TimeZones)
            {
                tz.Calendar = this;
                tz.Serialize(wrtr);
            }
            foreach (var e in Events)
            {
                e.Calendar = this;
                e.Serialize(wrtr);
            }
            foreach (var td in ToDos)
            {
                td.Calendar = this;
                td.Serialize(wrtr);
            }
            foreach (var fb in FreeBusy)
            {
                fb.Calendar = this;
                fb.Serialize(wrtr);
            }
            foreach (var jn in JournalEntries)
            {
                jn.Calendar = this;
                jn.Serialize(wrtr);
            }
            wrtr.EndBlock("VCALENDAR");
        }

        public void Save(CALDAVContext mDb, Guid folderId, Guid fileId)
        {
			this.ValidateModel();
			
            // eager load the calendar file details
            CalendarFile calendarFile = mDb.CalendarFile
                .Include(a => a.FolderNavigation.CalendarFolderInfo)
                .Include(a => a.CalendarEvent)
                .Include(a => a.CalendarFreeBusy)
                .Include(a => a.CalendarJournal)
                .Include(a => a.CalendarTimeZone)
                .Include(a => a.CalendarToDo)
                .Where(a => a.FolderId == folderId && a.CalendarFileId == fileId).SingleOrDefault();

            // Create Calendar File
            if (calendarFile == null)
            {
                calendarFile = new CalendarFile
                {
                    FolderId = folderId,
                    CalendarFileId = fileId,

                    Created = DateTime.UtcNow
                };

                var calendarFolder = mDb.FolderInfo.Include(a => a.CalendarFolderInfo)
                    .Where(a => a.FolderId == folderId).SingleOrDefault();

                if (calendarFolder == null)
                    throw new Exception($"Calendar folder for folder ID: ({folderId.ToString()}) does not exist in DB context.");

                calendarFolder.Modified = DateTime.UtcNow;
                calendarFolder.CalendarFolderInfo.Modified = DateTime.UtcNow;

                mDb.CalendarFile.Add(calendarFile);
            }
            else
            {
                calendarFile.FolderNavigation.Modified = DateTime.UtcNow;
                // used to trigger CTag RowVersion Update
                calendarFile.FolderNavigation.CalendarFolderInfo.Modified = DateTime.UtcNow;
            }

            // update Calendar File

            calendarFile.Scale = this.Scale;
            calendarFile.ProdId = this.ProdID;
            calendarFile.Modified = DateTime.UtcNow;

            foreach (var e in Events)
            {
                e.Calendar = this;
                e.Save(mDb, calendarFile);
            }
            foreach (var td in ToDos)
            {
                td.Calendar = this;
                td.Save(mDb, calendarFile);
            }

            /*
            foreach (var tz in TimeZones)
            {
                tz.Calendar = this;
                tz.Save(mDb, calendarFile);
            }
            foreach (var fb in FreeBusy)
            {
                fb.Calendar = this;
                fb.Save(mDb, calendarFile);
            }
            foreach (var jn in JournalEntries)
            {
                jn.Calendar = this;
                jn.Save(mDb, calendarFile);
            }
            */

            mDb.SaveChanges();
        }

        public void Delete(CALDAVContext mDb, Guid folderId, Guid fileId)
        {

            // eager load the calendar file details
            CalendarFile calendarFile = mDb.CalendarFile
                .Include(a => a.FolderNavigation.CalendarFolderInfo)
                .Include(a => a.CalendarEvent)
                .Include(a => a.CalendarFreeBusy)
                .Include(a => a.CalendarJournal)
                .Include(a => a.CalendarTimeZone)
                .Include(a => a.CalendarToDo)
                .Where(a => a.FolderId == folderId && a.CalendarFileId == fileId).SingleOrDefault();

            
            if (calendarFile == null)
            {
                return ;

            }
            // Update Folder CTag
            else
            {
                calendarFile.FolderNavigation.Modified = DateTime.UtcNow;
                // used to trigger CTag RowVersion Update
                calendarFile.FolderNavigation.CalendarFolderInfo.Modified = DateTime.UtcNow;
            }

            
            foreach (var e in Events)
            {
                e.Calendar = this;
                e.Delete(mDb, calendarFile);
            }
            foreach (var td in ToDos)
            {
                td.Calendar = this;
                td.Delete(mDb, calendarFile);
            }

            /*
            foreach (var tz in TimeZones)
            {
                tz.Calendar = this;
                tz.Delete(mDb, calendarFile);
            }
            foreach (var fb in FreeBusy)
            {
                fb.Calendar = this;
                fb.Delete(mDb, calendarFile);
            }
            foreach (var jn in JournalEntries)
            {
                jn.Calendar = this;
                jn.Delete(mDb, calendarFile);
            }
            */

            if (calendarFile != null)
            {
                mDb.Remove(calendarFile);
            }


            mDb.SaveChanges();
        }

        public IQueryable<ICalendarObject> Filter(CALDAVContext mDb, Guid folderId, Filter filer)
        {
            throw new NotImplementedException();
        }

    }
}
