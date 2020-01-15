using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem.ACL;
using System.Xml.Linq;

namespace CalDav.Models.Requests
{
    public class CalDavRequestData
    {
        public int Depth { get; set; }
        public XDocument XmlBody { get; set; }

        public IPrincipalItem CurrentPrincipal { get; set; }

        public Calendar CurrentCalendar { get; set; }
    }
}
