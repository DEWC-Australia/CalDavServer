using System;
using System.Collections.Generic;
using System.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class Put : CalDavRequest
    {

        private CalDavRequestData RequestData { get; set; }
        private AbstractFolder Folder { get; set; }

        public Put(CalDavRequestData requestData, AbstractFolder folder) 
        {
            RequestData = requestData;
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

                
                var e = RequestData.CurrentCalendar.Items.FirstOrDefault();
                e.LastModified = DateTime.UtcNow;
                System.Net.HttpStatusCode result = calendarFolder.Save(RequestData.CurrentCalendar, e.UID);

                return new Result()
                {
                    Headers = new Dictionary<string, string> {
                                {"Location", $"{Folder.Path}{e.UID}.ics" },
                                {"ETag", Common.FormatDate(e.LastModified) }
                    },
                    Status = result
                };
            }

            NotImplemented notImplemented = new NotImplemented();

            return notImplemented.Process();
        }

    }
}
