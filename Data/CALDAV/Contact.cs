using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class Contact
    {
        public Contact()
        {
            CalendarEvent = new HashSet<CalendarEvent>();
            CalendarFreeBusy = new HashSet<CalendarFreeBusy>();
            CalendarJournal = new HashSet<CalendarJournal>();
        }

        public Guid ContactId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SentBy { get; set; }
        public string Directory { get; set; }

        public virtual ICollection<CalendarEvent> CalendarEvent { get; set; }
        public virtual ICollection<CalendarFreeBusy> CalendarFreeBusy { get; set; }
        public virtual ICollection<CalendarJournal> CalendarJournal { get; set; }
    }
}
