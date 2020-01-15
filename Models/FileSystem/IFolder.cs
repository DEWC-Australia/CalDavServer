using System.Collections.Generic;

namespace CalDav.Models.FileSystem
{
    public interface IFolder
    {
        List<IFileSystemInfo> Items { get; set; }
        List<IFolder> Folders { get; set; }
    }
}
