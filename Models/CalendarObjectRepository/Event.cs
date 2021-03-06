﻿using CalDav.Data.CALDAV;
using CalDav.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace CalDav.CalendarObjectRepository
{
    public class Event : ObjectBase, ICalendarObject
    {
        private DateTime DTSTAMP = DateTime.UtcNow;

        public Event()
        {
            Attendees = new List<Contact>();
            Alarms = new List<Alarm>();
            Categories = new List<string>();
            Recurrences = new List<Recurrence>();
            Properties = new List<Tuple<string, string, System.Collections.Specialized.NameValueCollection>>();
            Attachments = new List<Uri>();
        }

        public virtual Calendar Calendar { get; set; }
        public virtual ICollection<Contact> Attendees { get; set; }
        public virtual ICollection<Alarm> Alarms { get; set; }
        public virtual ICollection<string> Categories { get; set; }
        public virtual ICollection<Uri> Attachments { get; set; }
        public virtual Classes? Class { get; set; }
        public virtual DateTime? Created { get; set; }
		[StringLength(int.MaxValue)]
		public virtual string Description { get; set; }
        public virtual bool IsAllDay { get; set; }
        public virtual DateTime? LastModified { get; set; }
        public virtual DateTime? Start { get; set; }
        public virtual DateTime? End { get; set; }
		[StringLength(255)]
		public virtual string Location { get; set; }
        public virtual int? Priority { get; set; }
        public virtual Statuses? Status { get; set; }
        public virtual int? Sequence { get; set; }
		[StringLength(int.MaxValue)]
		public virtual string Summary { get; set; }
		[StringLength(255)]
		public virtual string Transparency { get; set; }
        public virtual string UID { get; set; }
		
		public virtual Uri Url { get; set; }
        public virtual Contact Organizer { get; set; }
        public virtual ICollection<Recurrence> Recurrences { get; set; }

        public ICollection<Tuple<string, string, System.Collections.Specialized.NameValueCollection>> Properties { get; set; }

        public void Deserialize(System.IO.TextReader rdr, Serializer serializer)
        {
            string name, value;
            var parameters = new System.Collections.Specialized.NameValueCollection();
            while (rdr.Property(out name, out value, parameters) && !string.IsNullOrEmpty(name))
            {
                switch (name.ToUpper())
                {
                    case "BEGIN":

                        switch (value)
                        {

                            
                            case "VALARM":

                                var a = serializer.GetService<Alarm>();

                                        a.Deserialize(rdr, serializer);
#warning Disabled VALARM serializer as is it causing crashes in CalDavSync on outlook
                                /*
                                        Alarms.Add(a);
                                    */
                                break;

                                }
                                break;
                    case "ATTENDEE":
                        var contact = new Contact();
                        contact.Deserialize(value, parameters);
                        Attendees.Add(contact);
                        break;
                    case "CATEGORIES":
                        Categories = value.SplitEscaped().ToList();
                        break;
                    case "CLASS": Class = value.ToEnum<Classes>(); break;
                    case "CREATED": Created = value.ToDateTime(); break;
                    case "DESCRIPTION": Description = value; break;
                    case "DTEND": End = value.ToDateTime(); break;
                    case "DTSTAMP": DTSTAMP = value.ToDateTime().GetValueOrDefault(); break;
                    case "DTSTART": Start = value.ToDateTime(); break;
                    case "LAST-MODIFIED": LastModified = value.ToDateTime(); break;
                    case "LOCATION": Location = value; break;
                    case "ORGANIZER":
                        Organizer = serializer.GetService<Contact>();
                        Organizer.Deserialize(value, parameters);
                        break;
                    case "PRIORITY": Priority = value.ToInt(); break;
                    case "SEQUENCE": Sequence = value.ToInt(); break;
                    case "STATUS": Status = value.ToEnum<Statuses>(); break;
                    case "SUMMARY": Summary = value; break;
                    case "TRANSP": Transparency = value; break;
                    case "UID": UID = value; break;
                    case "URL": Url = value.ToUri(); break;
                    case "ATTACH":
                        var attach = value.ToUri();
                        if (attach != null)
                            Attachments.Add(attach);
                        break;
                    case "RRULE":
                        var rule = serializer.GetService<Recurrence>();
                        rule.Deserialize(null, parameters);
                        Recurrences.Add(rule);
                        break;
                    case "END": return;
                    default:
                        Properties.Add(Tuple.Create(name, value, parameters));
                        break;
                }
            }

            IsAllDay = Start == End;
        }

        public DateTime DateTimeHelper(string date)
        {
            int year = int.Parse(date.Substring(0, 4));
            int month = int.Parse(date.Substring(4, 2));
            int day = int.Parse(date.Substring(6, 2));
            return new DateTime(year, month, day);
        }

        public void Serialize(System.IO.TextWriter wrtr)
        {
            if (End != null && Start != null && End < Start)
            {
                End = Start;
            }

            wrtr.BeginBlock("VEVENT");
            wrtr.Property("UID", UID);

            if (Attendees != null)
            {
                foreach (var attendee in Attendees)
                {
                    wrtr.Property("ATTENDEE", attendee);
                }
            }

            if (Categories != null && Categories.Count > 0)
            {
                wrtr.Property("CATEGORIES", Categories);
            }

            wrtr.Property("CLASS", Class);
            wrtr.Property("CREATED", Created);
            wrtr.Property("DESCRIPTION", Description);
            wrtr.Property("DTEND", IsAllDay ? (End ?? Start.Value).Date : End);
            wrtr.Property("DTSTAMP", DTSTAMP);
            wrtr.Property("DTSTART", IsAllDay ? (Start ?? End.Value).Date : Start);
            wrtr.Property("LAST-MODIFIED", LastModified);
            wrtr.Property("LOCATION", Location);
            wrtr.Property("ORGANIZER", Organizer);
            wrtr.Property("PRIORITY", Priority);
            wrtr.Property("SEQUENCE", Sequence);
            wrtr.Property("STATUS", Status);
            wrtr.Property("SUMMARY", Summary);
            wrtr.Property("TRANSP", Transparency);
            wrtr.Property("URL", Url);

            if (Properties != null)
            {
                foreach (var prop in Properties)
                {
                    wrtr.Property(prop.Item1, prop.Item2, parameters: prop.Item3);
                }
            }

            if (Alarms != null)
            {
                foreach (var alarm in Alarms)
                {
                    alarm.Serialize(wrtr);
                }
            }
            
            wrtr.EndBlock("VEVENT");
        }

        protected Guid FileUid()
        {
            Guid result = new Guid();

            if(Guid.TryParse(this.UID, out result))
            {
                return result;
            }

            throw new Exception("Calendar UID is not a valid Guid.");
        }

        public void Save(CALDAVContext mDb, CalendarFile calendarFile)
        {
			this.ValidateModel();	
            // assuming all events come with a UID
            // Create Event for the file
            var calendarEvent = calendarFile.CalendarEvent.Where(a => a.EventId == this.FileUid()).SingleOrDefault();
            if (calendarEvent == null)
            {
                calendarEvent = new CalendarEvent
                {
                    FolderId = calendarFile.FolderId,
                    CalendarFileId = calendarFile.CalendarFileId,
                    EventId = this.FileUid()
                };
                mDb.CalendarEvent.Add(calendarEvent);
            }

            //calendarEvent.Class = this.Class;
            calendarEvent.Created = this.Created;
            if((this.Description ?? "").Length <= (int.MaxValue/2))
                calendarEvent.Description = this.Description;

            calendarEvent.End = this.End;
            calendarEvent.IsAllDay = this.IsAllDay;
            calendarEvent.Location = this.Location;
            calendarEvent.Modified = this.LastModified;
            //calendarFile.CalendarEvent.Organizer = this.Organizer;
            calendarEvent.Priority = this.Priority;
            calendarEvent.Sequence = this.Sequence;
            calendarEvent.Start = this.Start;
            //calendarFile.CalendarEvent.Status = this.Status;
            calendarEvent.Summary = this.Summary;
            calendarEvent.Transparency = this.Transparency;
            if(this.Url != null)
            {
                calendarEvent.Url = this.Url.LocalPath;
            }
            

        }

        public void Delete(CALDAVContext mDb, CalendarFile calendarFile)
        {
            var calendarEvent = calendarFile.CalendarEvent.Where(a => a.EventId == this.FileUid()).SingleOrDefault();
            if (calendarEvent != null)
            {
                mDb.CalendarEvent.Remove(calendarEvent);
            }
            
        }

        public IQueryable<ICalendarObject> Filter(CALDAVContext mDb, CalendarFile calendarFile, Filter filer)
        {
            throw new NotImplementedException();
        }
    }


}
