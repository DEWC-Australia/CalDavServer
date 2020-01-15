using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class UserFolderAccess
    {
        public Guid UserId { get; set; }
        public Guid FolderId { get; set; }
        public string Path { get; set; }
        public bool AccessControl { get; set; }
        public bool CalendarAccess { get; set; }
        public bool CalendarServerSharing { get; set; }
        public bool Owner { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }

        public virtual FolderInfo Folder { get; set; }
        public virtual UserProfile User { get; set; }
    }
}
