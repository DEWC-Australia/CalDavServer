using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalendarFreeBusy
    {
        public Guid FolderId { get; set; }
        public Guid CalendarFileId { get; set; }
        public Guid FreeBusyId { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public string Url { get; set; }
        public int? Sequence { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Guid? Organizer { get; set; }

        public virtual CalendarFile CalendarFile { get; set; }
        public virtual Contact OrganizerNavigation { get; set; }
    }
}
