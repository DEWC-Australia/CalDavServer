using CalDav.Data.CALDAV;
using System;
using System.Linq;

namespace CalDav.CalendarObjectRepository
{
    public interface ICalendarObject : ISerializeToICAL
    {
        string UID { get; set; }
        int? Sequence { get; set; }
        DateTime? LastModified { get; set; }
        Calendar Calendar { get; set; }

        void Save(CALDAVContext mDb, CalendarFile calendarFile);

        void Delete(CALDAVContext mDb, CalendarFile calendarFile);

        IQueryable<ICalendarObject> Filter(CALDAVContext mDb, CalendarFile calendarFile, Filter filer);

    }
}
