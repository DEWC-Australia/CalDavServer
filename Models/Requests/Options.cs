using System;
using System.Collections.Generic;
using System.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class Options : CalDavRequest
    {
        private CalDavRequestData RequestData { get; set; }
        private AbstractFolder Folder { get; set; }
        public Options(CalDavRequestData requestData, AbstractFolder folder)
        {
            RequestData = requestData;
            Folder = folder;
        }

        public override ActionResult Process()
        {

            var xdoc = RequestData.XmlBody;

            if (xdoc != null && (Folder.Type == FileSystem.FolderType.CalendarFolder))
            {
                var request = xdoc.Root.Elements().FirstOrDefault();

                AbstractCalendarRepository calendarFolder = (AbstractCalendarRepository)Folder;

                
                switch (request.Name.LocalName.ToLower())
                {
                    case "calendar-collection-set":
                        var calendars = this.RequestData.CurrentPrincipal.CalendarPaths;
                        
                        return new Result
                        {
                            Content = Common.xDav.Element("options-response",
                             Common.xCalDav.Element("calendar-collection-set",
                                 calendars.Select(calendar =>
                                     Common.xDav.Element("href",$"{calendar}"))
                             )
                         )
                        };
                }
                
            }

            return new Result
            {
                DavHeader = Folder.GetOptions,
            Headers = new Dictionary<string, string> {
                
                {"Allow", Folder.AllowOptions },
                {"Public", Folder.PublicOptions }
            }
        };
        
    }

    }
}
