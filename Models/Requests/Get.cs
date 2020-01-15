using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class Get : CalDavRequest
    {
        private AbstractFolder Folder { get; set; }
        public Get(AbstractFolder folder)
        {
            Folder = folder;
        }

        public override ActionResult Process()
        {
            if (Folder.Type == FileSystem.FolderType.CalendarFolder)
            {
                AbstractCalendarRepository calendarFolder = (AbstractCalendarRepository)Folder;

                var obj = calendarFolder.GetObjectByUID(calendarFolder.File);

                return new Result(System.Text.Encoding.UTF8.GetBytes(ToString(obj)))
                {
                    ContentType = "text/calendar",
                    Status = System.Net.HttpStatusCode.OK
                };
                
            }
            NotImplemented notImplemented = new NotImplemented();

            return notImplemented.Process();

            
        }
    }
}
