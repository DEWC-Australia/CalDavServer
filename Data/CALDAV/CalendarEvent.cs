using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalendarEvent
    {
        public Guid FolderId { get; set; }
        public Guid CalendarFileId { get; set; }
        public Guid EventId { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Created { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Location { get; set; }
        public int? Priority { get; set; }
        public int? Sequence { get; set; }
        public string Summary { get; set; }
        public string Transparency { get; set; }
        public string Url { get; set; }
        public int? ClassId { get; set; }
        public Guid? Organizer { get; set; }
        public int? StatusId { get; set; }

        public virtual CalendarFile CalendarFile { get; set; }
        public virtual Class Class { get; set; }
        public virtual Contact OrganizerNavigation { get; set; }
        public virtual Statuses Status { get; set; }
    }
}
