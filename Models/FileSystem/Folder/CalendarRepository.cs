using System;
using CalDav.Data.CALDAV;

namespace CalDav.Models.FileSystem.Folder
{
    public class CalendarRepository : AbstractCalendarRepository
    {
        
        public CalendarRepository(CALDAVContext db, UserFolderAccess userAccess, FolderInfo folderInfo, CalDavServer server, Guid file) : base(db, userAccess, folderInfo, server, file)
        {
            
        }

    }
}
