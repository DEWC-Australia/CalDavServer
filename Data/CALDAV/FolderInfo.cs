using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class FolderInfo
    {
        public FolderInfo()
        {
            CalDavServer = new HashSet<CalDavServer>();
            CalendarFile = new HashSet<CalendarFile>();
            InverseParentFolder = new HashSet<FolderInfo>();
            UserFolderAccess = new HashSet<UserFolderAccess>();
            UserProfileAclFolderNavigation = new HashSet<UserProfile>();
            UserProfileCalendarHomeSetNavigation = new HashSet<UserProfile>();
        }

        public Guid FolderId { get; set; }
        public string Path { get; set; }
        public Guid? ParentFolderId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int FolderType { get; set; }
        public bool Paging { get; set; }

        public virtual FolderType FolderTypeNavigation { get; set; }
        public virtual FolderInfo ParentFolder { get; set; }
        public virtual CalendarFolderInfo CalendarFolderInfo { get; set; }
        public virtual ICollection<CalDavServer> CalDavServer { get; set; }
        public virtual ICollection<CalendarFile> CalendarFile { get; set; }
        public virtual ICollection<FolderInfo> InverseParentFolder { get; set; }
        public virtual ICollection<UserFolderAccess> UserFolderAccess { get; set; }
        public virtual ICollection<UserProfile> UserProfileAclFolderNavigation { get; set; }
        public virtual ICollection<UserProfile> UserProfileCalendarHomeSetNavigation { get; set; }
    }
}
