using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalDavServer
    {
        public Guid SystemId { get; set; }
        public string AllowOptions { get; set; }
        public string PublicOptions { get; set; }
        public Guid ContextPath { get; set; }
        public string BaseAclPath { get; set; }
        public string BaseCalendarHomeSetPath { get; set; }
        public bool DavLevel1 { get; set; }
        public bool DavLevel2 { get; set; }
        public bool DavLevel3 { get; set; }
        public bool Active { get; set; }

        public virtual FolderInfo ContextPathNavigation { get; set; }
    }
}
