using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class Class
    {
        public Class()
        {
            CalendarEvent = new HashSet<CalendarEvent>();
            CalendarJournal = new HashSet<CalendarJournal>();
            CalendarToDo = new HashSet<CalendarToDo>();
        }

        public int ClassId { get; set; }
        public string Value { get; set; }

        public virtual ICollection<CalendarEvent> CalendarEvent { get; set; }
        public virtual ICollection<CalendarJournal> CalendarJournal { get; set; }
        public virtual ICollection<CalendarToDo> CalendarToDo { get; set; }
    }
}
