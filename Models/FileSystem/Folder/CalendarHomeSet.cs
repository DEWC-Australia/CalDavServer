using System;
using CalDav.Data.CALDAV;

namespace CalDav.Models.FileSystem.Folder
{
    class CalendarHomeSet : AbstractFolder
    {
        public CalendarHomeSet(UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server) : base(userAccess, folderInfo, server)
        {
        }
    }
}
