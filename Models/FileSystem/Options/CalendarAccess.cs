using CalDav.Models.FileSystem.ACL;

namespace CalDav.Models.FileSystem.Options
{
    public class CalendarAccess : IPrincipleItemOption
    {
        public int Order => 5;
        public string Name { get { return "calendar-access"; } }
    }
}
