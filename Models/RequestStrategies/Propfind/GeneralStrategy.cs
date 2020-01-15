using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem.Folder;
using CalDav.Models.Requests;
using CalDav.Models.Requests.Properties;
using CalDav.Models.Requests.Properties.Propfind;
using CalDav.Models.Server;

namespace CalDav.Models.RequestStrategies.Propfind
{
    public class GeneralStrategy : IStrategy
    {
        private string Path { get; set; }
        private string CurrentUser { get; set; }

        private ServerFacade ServerDefintion { get; set; }
        private CalDavRequestData RequestData { get; set; }
        private CALDAVContext mDb { get; set; }
        private AbstractFolder Folder { get; set; }

        private List<PropfindProperty> Properties { get; set; }
        

        public GeneralStrategy(CalDavRequestData requestData, AbstractFolder folder)
        {
            RequestData = requestData;
            Folder = folder;
            LoadProperties();
        }

        private void LoadProperties()
        {
            Properties = new List<PropfindProperty>();
            Properties.Add(new CalendarColorProperty());
            Properties.Add(new CalendarDescriptionProperty());
            Properties.Add(new CalendarHomeSetProperty());
            Properties.Add(new CalendarUserAddressSetProperty());
            Properties.Add(new CurrentUserPrincipalProperty());
            Properties.Add(new CurrentUserPrivilegeSetProperty());
            Properties.Add(new DisplayNameProperty());
            Properties.Add(new GetContentTypeProperty());
            Properties.Add(new GetCtagProperty());
            Properties.Add(new SyncTokenProperty());
            Properties.Add(new GetCalendarFolderEtagProperty()); //added to support busycal
            Properties.Add(new OwnerProperty());
            Properties.Add(new PrincipalUrlProperty());
            Properties.Add(new ResourceTypeProperty());
            Properties.Add(new SupportedCalendarComponentSetProperty());
            Properties.Add(new SupportedReportSetProperty());
        }


        public Result DoOperation()
        {
			var prop = (RequestData.XmlBody == null) ? null : RequestData.XmlBody.Descendants(Common.xDav.GetName("prop")).FirstOrDefault();
            var elementBase = Common.xDav.Element("multistatus");
            var depth = RequestData.Depth;
            if (prop != null)
            {
                BuildResponse(Folder, prop.Elements().ToList(), depth, elementBase);

                return new Result
                {
                    Status = (System.Net.HttpStatusCode)207,
                    DavHeader = Folder.GetOptions,
                    Content = elementBase
                };
            }
            else
            {
                var response = Common.xDav.Element("response",
                Common.xDav.Element("href", Folder.Path),
                Common.xDav.Element("propstat",
                    Common.xDav.Element("status", "HTTP/1.1 200 OK")));

                return new Result
                {
                    Status = (System.Net.HttpStatusCode)207,
                    DavHeader = Folder.GetOptions,
                    Content = Common.xDav.Element("multistatus", response, null)
                };

            }

        }


        private void BuildResponse(AbstractFolder currentFolder, List<XElement> props, int depth, XElement elementBase)
        {
            var supportedProperties = new HashSet<XName>();          

            var response = Common.xDav.Element("response");
            response.Add(Common.xDav.Element("href", currentFolder.Path));
            var propStat = Common.xDav.Element("propstat");
            response.Add(propStat);

            propStat.Add(Common.xDav.Element("status", "HTTP/1.1 200 OK"));
            
            var prop = Common.xDav.Element("prop");
            propStat.Add(prop);


            if (depth < 0)
                return;

            AbstractCalendarRepository calendarFolder = null;


            var allprop = props.Elements(Common.xDav.GetName("allprops")).Any();
            var hrefName = Common.xDav.GetName("href");
            var privilegeName = Common.xDav.GetName("privilege");

            XElement resourceType = null;
            XName resourceTypeName = null;

            XElement getctag = null;
            XName getctagName = null;
            
            XElement getContentType = null;
            XName getContentTypeName = null;

            XName getetagName = Common.xDav.GetName("getetag");


            // Dav:: https://tools.ietf.org/html/rfc3744#section-5.1.1
            // CalDav:: https://icalendar.org/RFC-Specifications/CalDAV-Access-RFC-4791/

            foreach (var currentProp in props)
            {
                var test = Properties.Where(a => a.Name == currentProp.Name).SingleOrDefault();

                XElement currentElement = null;
                if (test != null) {
                    currentElement = test.Build(allprop, props, currentFolder, prop, supportedProperties, RequestData);

                    if (currentProp.Name.LocalName.Equals("resourcetype"))
                    {
                        resourceType = currentElement;
                        resourceTypeName = test.Name;
                    }
                    else if (currentProp.Name.LocalName.Equals("getctag"))
                    {
                        getctag = currentElement;
                        getctagName = test.Name;
                    }
                    else if (currentProp.Name.LocalName.Equals("getcontenttype"))
                    {
                        getContentType = currentElement;
                        getContentTypeName = test.Name;
                    }
                }
            }

          
            var status = BuildUnsupportedProperties(props, supportedProperties);

            if (status != null)
                response.Add(status);

 
            elementBase.Add(response);
            // solve Depth
            
            // if this is a calendar folder and depth request remains return calendar props 
            if (currentFolder.Type == FileSystem.FolderType.CalendarFolder && depth > 0)
            {
                calendarFolder = (AbstractCalendarRepository)currentFolder;

                var calendarObjects = calendarFolder.GetObjects().Where(x => x != null).ToArray();

                var calendarItems = calendarObjects.Select(item => Common.xDav.Element("response",
                   hrefName.Element($"{calendarFolder.Path}{item.UID}.ics"),
                   Common.xDav.Element("propstat",
                       Common.xDav.Element("status", "HTTP/1.1 200 OK"),
                       resourceType == null ? null : resourceTypeName.Element(),
                       (getContentType == null
                           ? null
                           : getContentTypeName.Element("text/calendar; component=v" + item.GetType().Name.ToLower())),
                        getetagName.Element("\"" + Common.FormatDate(item.LastModified) + "\"")
                       ))).ToArray();

                elementBase.Add(calendarItems);
            }
            depth--;
            foreach (var child in currentFolder.ChildFolders ?? new List<AbstractFolder>())
            {
                BuildResponse(child, props, depth, elementBase);
            }


        }

        private Object BuildUnsupportedProperties(List<XElement> props, HashSet<XName> supportedProperties)
        {
            var propStat404 = Common.xDav.Element("propstat");
            propStat404.Add(Common.xDav.Element("status", "HTTP/1.1 404 Not Found"));


            var prop404 = Common.xDav.Element("prop");
            propStat404.Add(prop404);
            prop404.Add(props.Where(p => !supportedProperties.Contains(p.Name)).Select(p => new XElement(p.Name)));

            Object status = prop404.Elements().Any() ? propStat404 : null;
          
            return status;
        }

    }
}

