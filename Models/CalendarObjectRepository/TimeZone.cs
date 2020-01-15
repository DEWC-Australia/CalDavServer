using CalDav.Data.CALDAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CalDav.CalendarObjectRepository
{
    public class TimeZone : List<TimeZoneDetail>, ISerializeToICAL
    {
        public virtual Calendar Calendar { get; set; }

        public void Deserialize(System.IO.TextReader rdr, Serializer serializer)
        {
            string name, value;
            var parameters = new System.Collections.Specialized.NameValueCollection();
            while (rdr.Property(out name, out value, parameters) && !string.IsNullOrEmpty(name))
            {
                switch (name)
                {
                    case "BEGIN":
                        var detail = serializer.GetService<TimeZoneDetail>();
                        detail.Type = value;
                        detail.Calendar = Calendar;
                        detail.Deserialize(rdr, serializer);
                        Add(detail);
                        break;
                    case "END":
                        if (value == "VTIMEZONE")
                            return;
                        break;
                }
            }

        }

        public void Serialize(System.IO.TextWriter wrtr)
        {
            if (Count == 0) return;
            wrtr.BeginBlock("VTIMEZONE");
            foreach (var detail in this)
            {
                detail.Calendar = Calendar;
                detail.Serialize(wrtr);
            }
            wrtr.EndBlock("VTIMEZONE");
        }

        public void Save(CALDAVContext mDb, CalendarFile calendarFile)
        {
            throw new NotImplementedException();
        }

        public void Delete(CALDAVContext mDb, CalendarFile calendarFile)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ICalendarObject> Filter(CALDAVContext mDb, CalendarFile calendarFile, Filter filer)
        {
            throw new NotImplementedException();
        }
    }
}
