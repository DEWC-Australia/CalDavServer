using System;
using System.Collections.Generic;
using System.Text;
using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem.ACL;

namespace CalDav.Models.FileSystem.Folder
{
    class PrincipalFolder : AbstractFolder
    {
        public IPrincipalItem Principal { get; }
        public PrincipalFolder(IPrincipalItem currentPrincipal, UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server) : base(userAccess, folderInfo, server)
        {
            Principal = currentPrincipal;
        }
    }
}
