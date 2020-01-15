using System;
using System.Collections.Generic;

namespace CalDav.Models.FileSystem
{
    public interface IFileSystemInfo
    {

        // needs to be built by the principal and system, may need to be moved out of here into a new interface
        List<IOption> Options { get; }
        FolderType Type { get; }

        //Gets the creation date of the item in repository expressed as the coordinated universal time (UTC).
        DateTime Created { get; }
        //Gets the last modification date of the item in repository expressed as the coordinated universal time (UTC).
        DateTime Modified { get; }
        //Gets the name of the item in repository.
        string Name { get; }
        //Unique item path in the repository relative to storage root.
        string Path { get; }

        bool Paging { get; }

    }
}
