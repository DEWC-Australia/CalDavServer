
namespace CalDav.Models.FileSystem.Options
{
    public class DAVLevel3 : ISystemOption
    {
        public int Order => 3;
        public string Name { get { return "3"; } }
    }
}
