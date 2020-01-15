
namespace CalDav.Models.FileSystem.Options
{
    class Paging : ISystemOption
    {
        public int Order => 7;
        public string Name { get { return "paging"; } }
    }
}
