using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class Recurrence
    {
        public Guid RecurrenceId { get; set; }
        public int? Count { get; set; }
        public int? Interval { get; set; }
        public DateTime? Until { get; set; }
        public string WeekStart { get; set; }
        public int? ByMonth { get; set; }
        public string ByDay { get; set; }
        public int? ByMonthDay { get; set; }
        public string BySetPos { get; set; }
        public int? FrequencyId { get; set; }

        public virtual Frequency Frequency { get; set; }
    }
}
