using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem.ACL;
using System;

namespace CalDav.Models.FileSystem.Folder
{
    public class ContextPath : AbstractFolder
    {
        public ContextPath(IPrincipalItem currentPrincipal, UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server) : base(userAccess, folderInfo, server)
        {
            Principal = currentPrincipal;
        }

        // Context Path knows aboth the Principal to be returned during discovery
        public IPrincipalItem Principal { get; }

    }
}
