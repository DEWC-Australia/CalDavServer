using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class Frequency
    {
        public Frequency()
        {
            Recurrence = new HashSet<Recurrence>();
        }

        public int FrequencyId { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Recurrence> Recurrence { get; set; }
    }
}
