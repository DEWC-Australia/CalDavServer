using CalDav.Data.CALDAV;
using CalDav.Models.Requests;
using CalDav.Models.FileSystem.ACL;
using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Http;
using System;

namespace CalDav.Models.Server
{
    public interface ICalDavServer
    {
        UserProfile GetCurrentUser(HttpRequest httpRequest);
        // Get the current users CalendarHomeset, Calendars along with each calendar's R/W permissions and their User Options
        IPrincipalItem GetPrincipalItem(UserProfile currentUser);
        // Check if the requested filepath is valid for the current PrincipalItem
        // return the IFileSystemInfo and System Options if the user is valid, else return null
        AbstractFolder GetFileSystemInfo(string path, int depth, IPrincipalItem principalItem);
        // Get the request type so that the request can be processed using the principalItem and fileSystemInfo
        CalDavRequest GetRequestType(string method, CALDAVContext mDb, CalDavRequestData requestData, AbstractFolder folder);
        // get the http headers and xml body to be processed by the request
        CalDavRequestData BuildRequestData(HttpRequest httpRequest, IPrincipalItem currentPrincipal);
    }
}
