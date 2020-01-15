
namespace CalDav.Models
{
    public class CalDavSettings
    {
        
        public const string fileExtension = ".ics";
        public const string SystemName = "DEWC.com";
        public const string Version = "V0.1";
        public const string Lang = "EN";
        public const string ProdId = "-//" + SystemName + "/iCal/" + Version + "/" + Lang;

        public const bool AllowMakeCalendar = false;

        public const string SERVERFILEPATH = "wwwroot\\Files";
        public const string SERVERLOGPATH = "wwwroot\\Log";
        public const string LOGFILENAME = "\\Log.txt";

        public class Methods
        {
            public const string OPTIONS = "OPTIONS";
            public const string PROPFIND = "PROPFIND";
            public const string PROPPATCH = "PROPPATCH";
            public const string REPORT = "REPORT";
            public const string DELETE = "DELETE";
            public const string PUT = "PUT";
            public const string MKCALENDAR = "MKCALENDAR";
            public const string GET = "GET";
        }

        /*
         * Site Root: https://www.dewc.com
         *  Context Path: /CALDAV/
         *  Home Set Folders:
         *      - calendars
         * 
         */

        public class RouteSettings
        {
            public const string SITEROOT = "https://www.dewc.com";

            public const string WELLKNOWNROUTE = ".well-known/caldav";
            public const string WELLKNOWN = "/" + WELLKNOWNROUTE;

            public const string CONTEXT = "CALDAV";
            public const string CONTEXTPATH = "/" + CONTEXT + "/";

            public const string BASEROUTE = CONTEXT + "/{*more}";

       }

        public class HttpHeader
        {
            public const string LOCATION = "Location";
            public const string CONTENTTYPE = "Content-Type";
            public const string XENVVERSION = "X-Env-Version";
            public const string XOSVERSION = "X-OS-Version";
            public const string XENGINE = "X-Engine";
            public const string DAV = "DAV";
        }

        public class HttpContentType
        {
            public const string CHARSETUTF8 = "charset=utf-8";
            public const string TEXTXML = "text/xml";
        }

    }
}
