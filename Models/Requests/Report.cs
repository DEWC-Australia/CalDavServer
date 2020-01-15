using System;
using System.Collections.Generic;
using System.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class Report : CalDavRequest
    {
        private CalDavRequestData RequestData { get; set; }
        private AbstractFolder Folder { get; set; }
 
        public Report(CalDavRequestData requestData, AbstractFolder folder)
        {
            RequestData = requestData;
            Folder = folder;
        }

        public override ActionResult Process()
        {
            var xdoc = RequestData.XmlBody;

            if (xdoc == null || !(Folder.Type == FileSystem.FolderType.CalendarFolder))
            {
                return new Result();
            }

            AbstractCalendarRepository calendarFolder = (AbstractCalendarRepository)Folder;

            var firstElement =xdoc.Root.Elements().FirstOrDefault();

            if(xdoc.Root.Name.LocalName == "sync-collection")
            {

                var multiStat = Common.xDav.Element("multistatus");
                multiStat.Add(Common.xDav.Element("sync-token", "http://CALDAV/sync/" + calendarFolder.Ctag));
                multiStat.Add(Common.xDav.Element("sync-level", RequestData.Depth));
                if(RequestData.Depth > 0)
                {
                    IQueryable<ICalendarObject> calObjects = calendarFolder.GetObjects();

                    // check depth == sync-level
                    // sync response for each file in the folder - depth = 1
                    /*
                     * <href>file href</href>
                     * <status>HTTP/1.1 200 OK</status>
                     * <propstat>
                     *      <prop>
                     *          <getetag>file Etag</getetag>
                     *      </prop>
                     * </propstat>
                     * 
                     * <sync-token>syncURL/GTag</sync-token>
                     */

                    if (calObjects != null)
                    {
                        return new Result
                        {
                            Status = (System.Net.HttpStatusCode)207,
                            Content = Common.xDav.Element("multistatus", 
                            Common.xDav.Element("sync-token", "http://CALDAV/sync/" + calendarFolder.Ctag), 
                            Common.xDav.Element("sync-level", RequestData.Depth),
                                calObjects.Select(r =>
                                    Common.xDav.Element("response",
                                        Common.xDav.Element("href", $"{Folder.Path}{r.UID}.ics"),
                                        Common.xDav.Element("propstat",
                                            Common.xDav.Element("status", "HTTP/1.1 200 OK"),
                                            Common.xDav.Element("prop",
                                                (Common.xDav.Element("getetag",
                                                        "\"" + Common.FormatDate(r.LastModified) + "\"")),
                                                (Common.xCalDav.Element("content-type", "text/calendar; charset=utf-8")
                                                )
                                            )
                                        )
                                    )
                                    )
                                    )
                        };
                    }
                }
                

            }
            else
            {
                var request = xdoc.Root.Elements().FirstOrDefault();
                var requests = xdoc.Root.Elements().ToList();

                var filterElm = requests.Where(a => a.Name == Common.xCalDav.GetName("filter")).SingleOrDefault();

                var filter = filterElm == null ? null : new Filter(filterElm);
                var hrefName = Common.xDav.GetName("href");
                var hrefs = xdoc.Descendants(hrefName).Select(x => x.Value).ToArray();
                var getetagName = Common.xDav.GetName("getetag");
                var getetag = xdoc.Descendants(getetagName).FirstOrDefault();
                var calendarDataName = Common.xCalDav.GetName("calendar-data");
                var calendarData = xdoc.Descendants(calendarDataName).FirstOrDefault();

                var ownerName = Common.xDav.GetName("owner");
                var displaynameName = Common.xDav.GetName("displayname");

                IQueryable<ICalendarObject> result = null;
                if (filter != null)
                {
                    result = calendarFolder.GetObjectsByFilter(filter);
                }
                else if (hrefs.All(a => a.Contains(".ics")))
                {
                    result = hrefs.Select(x => calendarFolder.GetObjectByUID(GetObjectUIDFromPath(x)))
                        .Where(x => x != null)
                        .AsQueryable();

                }
                else if (hrefs.All(a => !a.Contains(".ics")))
                {
                    result = calendarFolder.GetObjects();
                }

                if (result != null)
                {
                    return new Result
                    {
                        Status = (System.Net.HttpStatusCode)207,
                        Content = Common.xDav.Element("multistatus",
                            result.Select(r =>
                                Common.xDav.Element("response",
                                    Common.xDav.Element("href", $"{Folder.Path}{r.UID}.ics"),
                                    Common.xDav.Element("propstat",
                                        Common.xDav.Element("status", "HTTP/1.1 200 OK"),
                                        Common.xDav.Element("prop",
                                            (getetag == null
                                                ? null
                                                : Common.xDav.Element("getetag",
                                                    "\"" + Common.FormatDate(r.LastModified) + "\"")),
                                            (calendarData == null
                                                ? null
                                                : Common.xCalDav.Element("calendar-data",
                                                    ToString(r)
                                                    ))
                                            )
                                        )
                                    )
                                ))
                    };
                }

            }

            return new Result
            {
                Headers = new Dictionary<string, string> {
                    {"ETag" , calendarFolder.Ctag }
                }
            };
        }

        

        protected virtual Guid GetObjectUIDFromPath(string path)
        {
            // fix: Regex
            var value = path.Split("/").Last().Split(".").First();
            Guid result = new Guid();
            if(Guid.TryParse(value,out result))
            {
                return result;
            }
            throw new Exception("File is not a Guid");
        }
    }
}
