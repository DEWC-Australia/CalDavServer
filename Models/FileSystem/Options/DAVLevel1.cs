
namespace CalDav.Models.FileSystem.Options
{
    public class DAVLevel1 : ISystemOption
    {
        public int Order => 1;
        public string Name { get { return "1"; } }
    }
}
