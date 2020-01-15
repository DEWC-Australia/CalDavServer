using CalDav.Data.CALDAV;
using System;

namespace CalDav.Models.FileSystem.Folder
{
    class WellKnownFolder : AbstractFolder
    {
        public WellKnownFolder(UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server) : base(userAccess, folderInfo, server)
        {

            ContextPath = folderInfo.ParentFolder.Path;
        }


        public string ContextPath { get; set; }

       
    }

    
}
