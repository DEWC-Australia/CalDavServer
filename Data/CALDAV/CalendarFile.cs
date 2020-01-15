using System;
using System.Collections.Generic;

namespace CalDav.Data.CALDAV
{
    public partial class CalendarFile
    {
        public CalendarFile()
        {
            CalendarEvent = new HashSet<CalendarEvent>();
            CalendarFreeBusy = new HashSet<CalendarFreeBusy>();
            CalendarJournal = new HashSet<CalendarJournal>();
            CalendarTimeZone = new HashSet<CalendarTimeZone>();
            CalendarToDo = new HashSet<CalendarToDo>();
        }

        public Guid FolderId { get; set; }
        public Guid CalendarFileId { get; set; }
        public string Version { get; set; }
        public string ProdId { get; set; }
        public string Scale { get; set; }
        public byte[] Etag { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public virtual CalendarFolderInfo Folder { get; set; }
        public virtual FolderInfo FolderNavigation { get; set; }
        public virtual ICollection<CalendarEvent> CalendarEvent { get; set; }
        public virtual ICollection<CalendarFreeBusy> CalendarFreeBusy { get; set; }
        public virtual ICollection<CalendarJournal> CalendarJournal { get; set; }
        public virtual ICollection<CalendarTimeZone> CalendarTimeZone { get; set; }
        public virtual ICollection<CalendarToDo> CalendarToDo { get; set; }
    }
}
