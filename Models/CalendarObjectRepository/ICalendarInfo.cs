using System;

namespace CalDav.CalendarObjectRepository
{
    public interface ICalendarInfo
    {
        string ID { get; }
        string Name { get; }
        string Description { get; }
        DateTime LastModified { get; }
    }
}
