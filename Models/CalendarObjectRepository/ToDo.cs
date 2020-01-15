using CalDav.Data.CALDAV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace CalDav.CalendarObjectRepository
{
    public class ToDo : ObjectBase, ICalendarObject
    {
		public virtual string UID { get; set; }
        internal DateTime? DTSTAMP;
        public virtual DateTime? Start { get; set; }
        public virtual DateTime? Due { get; set; }
		[StringLength(int.MaxValue)]
        public virtual string Summary { get; set; }
        public virtual Classes? Class { get; set; }
        public virtual ICollection<string> Categories { get; set; }
        public virtual int? Priority { get; set; }
        public virtual Statuses? Status { get; set; }
        public Calendar Calendar { get; set; }
        public virtual int? Sequence { get; set; }
        public virtual DateTime? LastModified { get; set; }
        public virtual DateTime? Completed { get; set; }
        public ICollection<Tuple<string, string, System.Collections.Specialized.NameValueCollection>> Properties { get; set; }

        public ToDo()
        {
            Categories = new List<string>();
            Properties = new List<Tuple<string, string, System.Collections.Specialized.NameValueCollection>>();
        }

        public void Deserialize(System.IO.TextReader rdr, Serializer serializer)
        {
            string name, value;
            var parameters = new System.Collections.Specialized.NameValueCollection();
            while (rdr.Property(out name, out value, parameters) && !string.IsNullOrEmpty(name))
            {
                switch (name.ToUpper())
                {
                    case "UID": UID = value; break;
                    case "DTSTAMP": DTSTAMP = value.ToDateTime(); break;
                    case "DTSTART": Start = value.ToDateTime(); break;
                    case "DUE": Due = value.ToDateTime(); break;
                    case "SUMMARY": Summary = value; break;
                    case "CLASS": Class = value.ToEnum<Classes>(); break;
                    case "CATEGORIES": Categories = value.SplitEscaped().ToList(); break;
                    case "PRIORITY": Priority = value.ToInt(); break;
                    case "STATUS": Status = value.ToEnum<Statuses>(); break;
                    case "LAST-MODIFIED": LastModified = value.ToDateTime(); break;
                    case "COMPLETED": Completed = value.ToDateTime(); break;
                    case "SEQUENCE": Sequence = value.ToInt(); break;
                    case "END": return;
                    default:
                        Properties.Add(Tuple.Create(name, value, parameters));
                        break;
                }
            }
        }

        public void Serialize(System.IO.TextWriter wrtr)
        {
            wrtr.BeginBlock("VTODO");
            wrtr.Property("UID", UID);
            wrtr.Property("DTSTAMP", DTSTAMP);
            wrtr.Property("DTSTART", Start);
            wrtr.Property("DUE", Due);
            wrtr.Property("SUMMARY", Summary);
            wrtr.Property("CLASS", Class);
            wrtr.Property("CATEGORIES", Categories);
            wrtr.Property("PRIORITY", Priority);
            wrtr.Property("STATUS", Status);
            wrtr.Property("SEQUENCE", Sequence);
            wrtr.Property("LAST-MODIFIED", LastModified);

            if (Properties != null)
                foreach (var prop in Properties)
                    wrtr.Property(prop.Item1, prop.Item2, parameters: prop.Item3);

            wrtr.EndBlock("VTODO");
        }

        public void Save(CALDAVContext mDb, CalendarFile calendarFile)
        {
			this.ValidateModel();
            // assuming all events come with a UID
            // Create Event for the file
            var calendarToDo = calendarFile.CalendarToDo.Where(a => a.ToDoId == this.FileUid()).SingleOrDefault();
            if (calendarToDo == null)
            {
                calendarToDo = new CalendarToDo();
                calendarToDo.FolderId = calendarFile.FolderId;
                calendarToDo.CalendarFileId = calendarFile.CalendarFileId;
                calendarToDo.ToDoId = this.FileUid();
                mDb.CalendarToDo.Add(calendarToDo);
            }

            //calendarToDo.Class = this.Class;
            calendarToDo.Completed = this.Completed;
            calendarToDo.DateTimeStamp = this.DTSTAMP;
            calendarToDo.Due = this.Due;
            calendarToDo.Modified = this.LastModified;
            calendarToDo.Priority = this.Priority;
            calendarToDo.Sequence = this.Sequence;
            calendarToDo.Start = this.Start;
            //calendarToDo.Status = this.Status;
            calendarToDo.Summary = this.Summary;

        }

        public void Delete(CALDAVContext mDb, CalendarFile calendarFile)
        {
            var calendarToDo = calendarFile.CalendarToDo.Where(a => a.ToDoId == this.FileUid()).SingleOrDefault();
            if (calendarToDo != null)
            {
                mDb.CalendarToDo.Remove(calendarToDo);
            }
        }

        public IQueryable<ICalendarObject> Filter(CALDAVContext mDb, CalendarFile calendarFile, Filter filer)
        {
            throw new NotImplementedException();
        }

        protected Guid FileUid()
        {
            Guid result = new Guid();

            if (Guid.TryParse(this.UID, out result))
            {
                return result;
            }

            throw new Exception("Calendar UID is not a valid Guid.");
        }

    }
}
