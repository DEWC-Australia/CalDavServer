using CalDav.Data.CALDAV;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CalDav.Models.FileSystem.ACL
{
    public interface IPrincipalItem
    {
        UserProfile CurrentUser { get; }
        string Path { get; }

        UserFolderAccess TestAccess(string path, CalDavServer server);
        String PrincipalURL { get; }

        String CalendarHomeSet { get; }

        List<string> CalendarPaths { get; }


    }
}
