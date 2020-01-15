using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalendarJournal
    {
        public Guid FolderId { get; set; }
        public Guid CalendarFileId { get; set; }
        public Guid JournalId { get; set; }
        public string Description { get; set; }
        public int? Sequence { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public Guid? Organizer { get; set; }
        public int? StatusId { get; set; }
        public int? ClassId { get; set; }

        public virtual CalendarFile CalendarFile { get; set; }
        public virtual Class Class { get; set; }
        public virtual Contact OrganizerNavigation { get; set; }
        public virtual Statuses Status { get; set; }
    }
}
