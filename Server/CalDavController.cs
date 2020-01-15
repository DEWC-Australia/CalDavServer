//https://www.codeproject.com/Articles/17980/Adding-iCalendar-Support-to-Your-Program-Part-1

using Microsoft.AspNetCore.Mvc;
using CalDav.Models;
using CalDav.Attributes;
using CalDav.Models.Server;
using CalDav.Data.CALDAV;
using System;
using CalDav.CalendarObjectRepository;
/*
 * To setup CALDAV
 * 1. Deploy the ASP.NET Core Web Project
 * 2. Setup Database
 * 3. Create the Files Folder within wwwroot and provide Read/Write permissions to the webserver
 *
 */
namespace CalDav.Controllers
{
    public abstract class CalDavController : Controller
    {
        
        private CALDAVContext mDb { get; set; }
        private ServerFacade Server { get; set; } 
        public CalDavController(CALDAVContext db)
        {
            mDb = db;
            Server = new ServerFacade(mDb);
        }

        // REPORT: Calendars/calendarName
        [BasicAuthorize("www.dewc.com")]
        [AcceptVerbs(new string[] { "REPORT", "PROPFIND", "PROPPATCH", "OPTIONS", "DELETE", "GET", "PUT", "MKCALENDAR" })]
        [Route(CalDavSettings.RouteSettings.WELLKNOWNROUTE)]
        [Route(CalDavSettings.RouteSettings.BASEROUTE)]
        public ActionResult CalDav()
        {
            try
            {

                // get the current username to retrieve their PrincipalItem
                var currentUser = Server.GetCurrentUser(this.Request);

                // USER NOT FOUND
                if (currentUser == null)
                    return new Result {
                        Status = System.Net.HttpStatusCode.Forbidden,
                        Content = "User Not Found",
                        ContentType = "text/plain"
                    };

                // retrieve the current user and the folders they can access
                // remember calendars are a folder containing many calendar files
                var principal = Server.GetPrincipalItem(currentUser);

                // check if the user has permission the access the current path request
                // need to return a based folder class
                var requestData = Server.BuildRequestData(this.Request, principal);

                var folder = Server.GetFileSystemInfo(this.Request.Path.Value, requestData.Depth, principal);

                // User cannot access the URL
                if (folder == null)
                    return new Result {
                        Status = System.Net.HttpStatusCode.Forbidden,
                        Content = $"User Access to {this.Request.Path.Value} Denied.",
                        ContentType = "text/plain"
                    };

                // get the HttpRequest Type for the to be processed
                var request = Server.GetRequestType(this.Request.Method, mDb, requestData, folder);

                // process the request and produce a Result to generate a Http Response
                return request.Process();
            }
            catch(Exception ex)
            {
                Exception test = ex;
                while (test.InnerException != null)
                {
                    test = test.InnerException;
                }

                
                var response = Common.xDav.Element("response");
                response.Add(Common.xDav.Element("href", this.Request.Path.Value));
                var propStat = Common.xDav.Element("propstat");
                propStat.Add(Common.xDav.Element("status", "HTTP/1.1 500 Internal Server Error"));
                propStat.Add(Common.xDav.Element("responsedescription", test.Message));
                response.Add(propStat);
                return new Result {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Content = Common.xDav.Element("multistatus", response, null)
                };
            }
        }
    }

}
