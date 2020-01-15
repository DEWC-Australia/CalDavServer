using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem.Folder;
using CalDav.Models.Requests;

namespace CalDav.Models
{
    public class CalDavRequestFactory
    {
        public static CalDavRequest GetRequest(string method, CALDAVContext mDb, CalDavRequestData requestData, AbstractFolder folder)
        {
            switch (method)
            {
                
                case CalDavSettings.Methods.OPTIONS: return new Options(requestData, folder);
                case CalDavSettings.Methods.PROPFIND: return new Propfind(requestData, folder);
                case CalDavSettings.Methods.PROPPATCH: return new Propfind(requestData, folder);
                case CalDavSettings.Methods.REPORT: return new Report(requestData, folder);
                case CalDavSettings.Methods.DELETE: return new Delete(folder);
                case CalDavSettings.Methods.PUT: return new Put(requestData, folder);
                case CalDavSettings.Methods.MKCALENDAR: return new MkCalendar(folder);
                case CalDavSettings.Methods.GET: return new Get(folder);
                
                default: return new NotImplemented();
            }
        }
    }
}
