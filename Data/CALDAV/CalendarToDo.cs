using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalendarToDo
    {
        public Guid FolderId { get; set; }
        public Guid CalendarFileId { get; set; }
        public Guid ToDoId { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? Due { get; set; }
        public string Summary { get; set; }
        public int? Priority { get; set; }
        public int? Sequence { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Completed { get; set; }
        public int? StatusId { get; set; }
        public int? ClassId { get; set; }

        public virtual CalendarFile CalendarFile { get; set; }
        public virtual Class Class { get; set; }
        public virtual Statuses Status { get; set; }
    }
}
