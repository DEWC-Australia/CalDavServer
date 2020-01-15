using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalDav.Models.FileSystem.ACL
{
    public class PrincipalItem : IPrincipalItem
    {
        public PrincipalItem(UserProfile currentUser, CALDAVContext db)
        {
            mDb = db;
            CurrentUser = currentUser;

        }

        protected List<string> BuildCalendarPaths()
        {
            return mDb.UserFolderAccess
                    .Include(a => a.Folder)
                    .Where(a => a.UserId.Equals(CurrentUser.UserId) && a.Read && a.Folder.FolderType == (int)FolderType.CalendarFolder)
                    .Select(a => a.Path).ToList(); 
        }

        protected CALDAVContext mDb { get; set; }

        public UserProfile CurrentUser { get; private set; }

        public string Path { get {
                return CurrentUser.AclFolderNavigation.Path;
            } }

      

        public UserFolderAccess TestAccess(string path, CalDavServer server)
        {
            // all users who are active in the database have access to the context path
            if (path.Equals(server.ContextPathNavigation.Path))              
                return mDb.UserFolderAccess.Where(a => a.UserId.Equals(CurrentUser.UserId) && a.Folder.Equals(server.ContextPath) && a.Read == true).SingleOrDefault();

            // user has access to their calendar homeset
            if (path.Equals(CurrentUser.CalendarHomeSetNavigation.Path))
                return mDb.UserFolderAccess.Where(a => a.UserId.Equals(CurrentUser.UserId) && a.Folder.Equals(CurrentUser.CalendarHomeSet) && a.Read == true).SingleOrDefault();
            
            // user has access to their ACL folder
            if (path.Equals(this.Path))
                  return mDb.UserFolderAccess.Where(a => a.UserId.Equals(CurrentUser.UserId) && a.Folder.Equals(CurrentUser.AclFolder) && a.Read == true).SingleOrDefault();
        
            return mDb.UserFolderAccess
                        .Include(a => a.Folder.ParentFolder)
                        .Include(a => a.Folder.CalendarFolderInfo)
                        .Where(a => a.Path.Equals(path) && a.UserId.Equals(CurrentUser.UserId) && a.Read == true)
                        .SingleOrDefault();
        }


        // know about the homeset folder for a user for Calendars to be discovered
        // For DEWC this will be /CalDav/Calendars/
        public String CalendarHomeSet => CurrentUser.CalendarHomeSetNavigation.Path;
        

        public String PrincipalURL => CurrentUser.AclFolderNavigation.Path;

        public List<string> CalendarPaths => BuildCalendarPaths();


    }
}
