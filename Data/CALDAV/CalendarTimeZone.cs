using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalendarTimeZone
    {
        public Guid FolderId { get; set; }
        public Guid CalendarFileId { get; set; }
        public Guid TimeZoneId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public long? OffsetFrom { get; set; }
        public long? OffsetTo { get; set; }

        public virtual CalendarFile CalendarFile { get; set; }
    }
}
