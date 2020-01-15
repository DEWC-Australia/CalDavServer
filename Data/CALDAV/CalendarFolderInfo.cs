using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalendarFolderInfo
    {
        public CalendarFolderInfo()
        {
            CalendarFile = new HashSet<CalendarFile>();
        }

        public Guid FolderId { get; set; }
        public string ContentType { get; set; }
        public DateTime Modified { get; set; }
        public byte[] Ctag { get; set; }
        public string CalendarColor { get; set; }
        public string CalendarDescription { get; set; }
        public bool AclPrincipalPropSet { get; set; }
        public bool PrincipalMatch { get; set; }
        public bool PrincipalPropertySearch { get; set; }
        public bool CalendarMultiGet { get; set; }
        public bool CalendarQuery { get; set; }

        public virtual FolderInfo Folder { get; set; }
        public virtual ICollection<CalendarFile> CalendarFile { get; set; }
    }
}
