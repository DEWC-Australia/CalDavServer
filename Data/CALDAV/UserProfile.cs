using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            UserFolderAccess = new HashSet<UserFolderAccess>();
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public Guid CalendarHomeSet { get; set; }
        public Guid AclFolder { get; set; }

        public virtual FolderInfo AclFolderNavigation { get; set; }
        public virtual FolderInfo CalendarHomeSetNavigation { get; set; }
        public virtual ICollection<UserFolderAccess> UserFolderAccess { get; set; }
    }
}
