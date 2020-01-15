using System.Collections.Generic;
using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class MkCalendar : CalDavRequest
    {
        private bool Enabled = true;
        private AbstractFolder Folder { get; set; }
        public MkCalendar(AbstractFolder folder) 
        {
        }

        public override ActionResult Process()
        {
            if (Folder.Type == FileSystem.FolderType.CalendarFolder && Enabled)
            {
                AbstractCalendarRepository calendarFolder = (AbstractCalendarRepository)Folder;

                var calendar = calendarFolder.CreateCalendar(calendarFolder.FolderID.ToString());

                return new Result
                {
                    Headers = new Dictionary<string, string> {
                    {"Location", calendarFolder.Path },
                },
                    Status = System.Net.HttpStatusCode.Created
                };
            }

            NotImplemented notImplemented = new NotImplemented();

            return notImplemented.Process();
            
        }

       
    }
}
