using CalDav.Models.FileSystem.ACL;

namespace CalDav.Models.FileSystem.Options
{
    public class CalendarServerSharing : IPrincipleItemOption
    {
        public int Order => 6;
        public string Name { get { return "calendarserver-sharing"; } }
    }
}
