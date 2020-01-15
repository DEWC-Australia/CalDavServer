using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class FolderType
    {
        public FolderType()
        {
            FolderInfo = new HashSet<FolderInfo>();
        }

        public int FolderType1 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<FolderInfo> FolderInfo { get; set; }
    }
}
