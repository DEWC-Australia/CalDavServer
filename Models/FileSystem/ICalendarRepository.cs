using CalDav.CalendarObjectRepository;
using System;
using System.Linq;

namespace CalDav.Models.FileSystem
{
    public interface ICalendarRepository
    {

        ICalendarInfo CreateCalendar(string id);
        System.Net.HttpStatusCode Save(Calendar ical, string UID);

        ICalendarObject GetObjectByUID(Guid uid);
        IQueryable<ICalendarObject> GetObjectsByFilter(Filter filter);
        IQueryable<ICalendarObject> GetObjects();

        void DeleteObject(Guid uid);
    }
}
