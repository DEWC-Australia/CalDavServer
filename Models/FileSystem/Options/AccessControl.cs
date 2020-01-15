using CalDav.Models.FileSystem.ACL;

namespace CalDav.Models.FileSystem.Options
{
    public class AccessControl : IPrincipleItemOption
    {
        public int Order => 4;
        public string Name { get { return "access-control"; } }
    }
}
