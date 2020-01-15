using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class DateTimeRange
    {
        public Guid DateTimeRangeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
