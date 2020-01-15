using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class Property
    {
        public Guid PropertyId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Parameters { get; set; }
    }
}
