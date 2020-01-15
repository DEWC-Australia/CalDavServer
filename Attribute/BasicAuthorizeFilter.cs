//https://www.johanbostrom.se/blog/adding-basic-auth-to-your-mvc-application-in-dotnet-core
namespace CalDav.Attributes
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using CalDav.Data.CALDAV;
    using CalDav.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using PasswordSecurity;

    public class BasicAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string realm;
        private CALDAVContext mDb { get; set; }
        public BasicAuthorizeFilter(CALDAVContext db, string realm = null)
        {
            mDb = db;
            this.realm = realm;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string username = null;
            string password = null;
            string authHeader = context.HttpContext.Request.Headers["Authorization"];

            /*
            if (context.HttpContext.Request.Headers["User-Agent"].Contains("CalDAV Sync Adapter (Android)"))
                return; 
            */
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {

                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                // Split username and password
                username = decodedUsernamePassword.Split(':', 2)[0];
                password = decodedUsernamePassword.Split(':', 2)[1];
                // Check if login is correct
                if (IsAuthorized(username, password))
                {
                    return;
                }
            }
            // Return authentication type (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic";
            // Add realm if it is not null
            if (!string.IsNullOrWhiteSpace(realm))
            {
                context.HttpContext.Response.Headers["WWW-Authenticate"] += $" realm=\"{realm}\"";
            }
            // Return unauthorized
            context.Result = new Result {
                Status = System.Net.HttpStatusCode.Unauthorized,
                Content = $"Login Credentials Failed for {username}",
                ContentType = "text/plain"
            };// UnauthorizedResult();
            
        }
        // Make your own implementation of this
        public bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct

            var testResult = mDb.UserProfile.Where(a => a.Active && a.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) && MD5Hash.verifyMd5Hash(password, a.Password)).SingleOrDefault();

            return (testResult != null);
        }
    }
}
