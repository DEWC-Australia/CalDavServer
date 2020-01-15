
namespace CalDav.Models.FileSystem.Options
{
    public class DAVLevel2 : ISystemOption
    {
        public int Order => 2;
        public string Name { get { return "2"; } }
    }
}
