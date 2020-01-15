using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class Delete : CalDavRequest
    {
        private AbstractFolder Folder { get; set; }
        public Delete(AbstractFolder folder) 
        {
            Folder = folder;
        }

        public override ActionResult Process()
        {
			if (!Folder.Write)
			{
				ForbiddenAccess forbidden = new ForbiddenAccess();
				return forbidden.Process();
			}

			if (Folder.Type == FileSystem.FolderType.CalendarFolder)
            {
                AbstractCalendarRepository calendarFolder = (AbstractCalendarRepository)Folder;

                calendarFolder.DeleteObject(calendarFolder.File);
                return new Result()
                {
                    Status = System.Net.HttpStatusCode.NoContent
                };
            }

            NotImplemented notImplemented = new NotImplemented();

            return notImplemented.Process();
        }
    }
}
