using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace CalDav.Models.Server
{


    public class CalDavPropertyFactory
    {
        // https://icalendar.org/RFC-Specifications/CalDAV-Access-RFC-4791/
        //urn:ietf:params:xml:ns:caldav
        //[[urn:ietf:params:xml:ns:caldav:calendar-description]]
        public XElement CalendarDescription { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:calendar-home-set]]
        public XElement CalendarHomeSet { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:calendar-timezone]]
        public XElement CalendarTimezone { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:filter]]
        public XElement Filter { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-attendees-per-instance]]
        public XElement MaxAttendeesPerInstance { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-date-time]]
        public XElement MaxDateTime { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-instances]]
        public XElement MaxInstances { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-resource-size]]
        public XElement MaxResourceSize { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:min-date-time]]
        public XElement MinDateTime { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:supported-calendar-component-set]]
        public XElement SupportedCalendarComponentSet { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:supported-calendar-data]]
        public XElement SupportedCalendarData { get; set; }
        //[[urn: ietf:params:xml:ns:supported-collation-set]]
        public XElement SupportedCollationSet { get; set; }
        //urn:ietf:params:xml:ns:caldav: calendar-user-address-set
        public XElement Comp { get; set; }

        //CalDAV: calendar-user-address-set
        //https://icalendar.org/CalDAV-Scheduling-RFC-6638/2-4-1-caldav-calendar-user-address-set-property.html
        public XElement CalendarUserAddressSet { get; set; }
        //CalDAV: calendar-user-type
        //https://icalendar.org/CalDAV-Scheduling-RFC-6638/2-4-2-caldav-calendar-user-type-property.html
        public XElement CalendarUserType { get; set; }

        //"http://apple.com/ns/ical/"
        public XElement CalendarColor { get; set; }
        public XElement CalendarOrder { get; set; }
        public XElement SupportedComponents { get; set; }
        public XElement CalendarData { get; set; }
        public XElement CalendarCollectionSet { get; set; }

        //https://github.com/dmfs/davwiki/wiki/http:--calendarserver.org-ns-:getctag
        //http://calendarserver.org/ns/
        public XElement Getctag { get; set; }

        
        /// https://tools.ietf.org/html/rfc3744#section-5.1.1
        /// </summary>
        // DAV:
        //[[DAV::activelock]]
        public XElement Activelock { get; set; }
        //[[DAV::allprop]]
        public XElement AllProps { get; set; }
        //[[DAV::collection]]
        public XElement Collection { get; set; }
        //[[DAV::creationdate]]
        public XElement CreationDate { get; set; }
        //[[DAV::current-user-principal]]
        public XElement CurrentUserPrincipal { get; set; }
        //[[DAV::current-user-privilege-set]]
        public XElement CurrentUserPrivilegeSet { get; }
        //[[DAV::depth]]
        //https://tools.ietf.org/html/rfc6578#section-3.3
        public XElement Depth { get; set; }
        //[[DAV::displayname]]
        public XElement DisplayName { get; set; }
        //[[DAV::error]]
        public XElement Error { get; set; }
        //[[DAV::exclusive]]
        public XElement Exclusive { get; set; }
        //[[DAV::getcontentlanguage]]
        public XElement GetContentLanguage { get; set; }
        //[[DAV::getcontentlength]]
        public XElement GetContentLength { get; set; }
        //[[DAV::getcontenttype]]
        public XElement GetContentType { get; set; }
        //[[DAV::getetag]]
        public XElement Getetag { get; set; }
        //[[DAV::getlastmodified]]
        public XElement GetLastModified { get; set; }
        //[[DAV::href]]
        public XElement Href { get; set; }
        //[[DAV::include]]
        public XElement Include { get; set; }
        //[[DAV::location]]
        public XElement Location { get; set; }
        //[[DAV::lockdiscovery]]
        public XElement Lockdiscovery { get; set; }
        //[[DAV::lockentry]]
        public XElement Lockentry { get; set; }
        //[[DAV::lockinfo]]
        public XElement Lockinfo { get; set; }
        //[[DAV::lockroot]]
        public XElement Lockroot { get; set; }
        //[[DAV::lockscope]]
        public XElement Lockscope { get; set; }
        //[[DAV::locktoken]]
        public XElement Locktoken { get; set; }
        //[[DAV::locktype]]
        public XElement Locktype { get; set; }
        //[[DAV::multistatus]]
        public XElement Multistatus { get; set; }
        //[[DAV::owner]]
        public XElement Owner { get; set; }
        //DAV: principal-URL http://sabre.io/dav/clients/ical/
        public XElement PrincipalURL { get; set; }
        //[[DAV::privilege]]
        public XElement Privilege { get; set; }
        //[[DAV::prop]]
        public XElement Prop { get; set; }
        //[[DAV::propertyupdate]]
        public XElement PropertyUpdate { get; set; }
        //[[DAV::propfind]]
        public XElement Propfind { get; set; }
        //[[DAV::propname]]
        public XElement Propname { get; set; }
        //[[DAV::propstat]]
        public XElement Propstat { get; set; }
        //[[DAV::read]]
        public XElement Read { get; set; }
        //[[DAV::remove]]
        public XElement Remove { get; set; }
        //[[DAV::resourcetype]]
        public XElement Resourcetype { get; set; }
        //[[DAV::response]]
        public XElement Response { get; set; }
        //[[DAV::responsedescription]]
        public XElement ResponseDescription { get; set; }
        //[[DAV::set]]
        public XElement Set { get; set; }
        //[[DAV::shared]]
        public XElement Shared { get; set; }
        //[[DAV::status]]
        public XElement Status { get; set; }
        //[[DAV::supportedlock]]
        public XElement SupportedLock { get; set; }
        //[[DAV:sync-collection]]
        //https://tools.ietf.org/html/rfc6578#section-6.1
        //https://tools.ietf.org/id/draft-daboo-webdav-sync-02.xml#rfc.section.4.2
        public XElement SyncCollection { get; set; }
        //[[DAV:: sync-level]]
        //https://github.com/sabre-io/dav/issues/1075
        //https://tools.ietf.org/html/rfc6578#section-6.3
        public XElement SyncLevel { get; set; }

        //[DAV:: sync-token]
        //https://tools.ietf.org/html/rfc6578#section-6.2
        //https://tools.ietf.org/html/rfc6578#section-3.3
        public XElement SyncToken { get; set; }
        //DAV: supported-report-set
        public XElement SupportedReportSet { get; set; }
        //[[DAV::timeout]]
        public XElement Timeout { get; set; }
        //[[DAV::write]]
        public XElement Write { get; set; }

    }
    public abstract class CalDavModel
    {

        public void GetElement(string name)
        {
            Type type = typeof(CalDavModel);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                
                object value = property.GetValue(this, null);
                // value is null skip
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                     continue;
            }

            
        }

        //urn:ietf:params:xml:ns:caldav
        //[[urn:ietf:params:xml:ns:caldav:calendar-description]]
        public XElement CalendarDescription { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:calendar-home-set]]
        public XElement CalendarHomeSet { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:calendar-timezone]]
        public XElement CalendarTimezone { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-attendees-per-instance]]
        public XElement MaxAttendeesPerInstance { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-date-time]]
        public XElement MaxDateTime { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-instances]]
        public XElement MaxInstances { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:max-resource-size]]
        public XElement MaxResourceSize { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:min-date-time]]
        public XElement MinDateTime { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:supported-calendar-component-set]]
        public XElement SupportedCalendarComponentSet { get; set; }
        //[[urn:ietf:params:xml:ns:caldav:supported-calendar-data]]
        public XElement SupportedCalendarData { get; set; }
        //[[urn: ietf:params:xml:ns:supported-collation-set]]
        public XElement SupportedCollationSet { get; set; }
        //urn:ietf:params:xml:ns:caldav: calendar-user-address-set
        public XElement Comp { get; set; }

        //CalDAV: calendar-user-address-set
        public XElement CalendarUserAddressSet { get; set; }
        
        
        //"http://apple.com/ns/ical/"
        public XElement CalendarColor { get; set; }
        public XElement CalendarOrder { get; set; }
        public XElement SupportedComponents { get; set; }
        public XElement CalendarData { get; set; }
        public XElement CalendarCollectionSet { get; set; }
        //http://calendarserver.org/ns/
        public XElement Getctag { get; set; }


        // DAV:
        //[[DAV::activelock]]
        public XElement Activelock { get; set; }
        //[[DAV::allprop]]
        public XElement AllProps { get; set; }
        //[[DAV::collection]]
        public XElement Collection { get; set; }
        //[[DAV::creationdate]]
        public XElement CreationDate { get; set; }
        //[[DAV::current-user-principal]]
        public XElement CurrentUserPrincipal { get; set; }
        //[[DAV::depth]]
        public XElement Depth { get; set; }
        //[[DAV::displayname]]
        public XElement DisplayName { get; set; }
        //[[DAV::error]]
        public XElement Error { get; set; }
        //[[DAV::exclusive]]
        public XElement Exclusive { get; set; }
        //[[DAV::getcontentlanguage]]
        public XElement GetContentLanguage { get; set; }
        //[[DAV::getcontentlength]]
        public XElement GetContentLength { get; set; }
        //[[DAV::getcontenttype]]
        public XElement GetContentType { get; set; }
        //[[DAV::getetag]]
        public XElement Getetag { get; set; }
        //[[DAV::getlastmodified]]
        public XElement GetLastModified { get; set; }
        //[[DAV::href]]
        public XElement Href { get; set; }
        //[[DAV::include]]
        public XElement Include { get; set; }
        //[[DAV::location]]
        public XElement Location { get; set; }
        //[[DAV::lockdiscovery]]
        public XElement Lockdiscovery { get; set; }
        //[[DAV::lockentry]]
        public XElement Lockentry { get; set; }
        //[[DAV::lockinfo]]
        public XElement Lockinfo { get; set; }
        //[[DAV::lockroot]]
        public XElement Lockroot { get; set; }
        //[[DAV::lockscope]]
        public XElement Lockscope { get; set; }
        //[[DAV::locktoken]]
        public XElement Locktoken { get; set; }
        //[[DAV::locktype]]
        public XElement Locktype { get; set; }
        //[[DAV::multistatus]]
        public XElement Multistatus { get; set; }
        //[[DAV::owner]]
        public XElement Owner { get; set; }
        //DAV: principal-URL http://sabre.io/dav/clients/ical/
        public XElement PrincipalURL { get; set; }
        //[[DAV::prop]]
        public XElement Prop { get; set; }
        //[[DAV::propertyupdate]]
        public XElement PropertyUpdate { get; set; }
        //[[DAV::propfind]]
        public XElement Propfind { get; set; }
        //[[DAV::propname]]
        public XElement Propname { get; set; }
        //[[DAV::propstat]]
        public XElement Propstat { get; set; }
        //[[DAV::remove]]
        public XElement Remove { get; set; }
        //[[DAV::resourcetype]]
        public XElement Resourcetype { get; set; }
        //[[DAV::response]]
        public XElement Response { get; set; }
        //[[DAV::responsedescription]]
        public XElement ResponseDescription { get; set; }
        //[[DAV::set]]
        public XElement Set { get; set; }
        //[[DAV::shared]]
        public XElement Shared { get; set; }
        //[[DAV::status]]
        public XElement Status { get; set; }
        //[[DAV::supportedlock]]
        public XElement SupportedLock { get; set; }
        //DAV: supported-report-set
        public XElement SupportedReportSet { get; set; }
        //[[DAV::timeout]]
        public XElement Timeout { get; set; }
        //[[DAV::write]]
        public XElement Write { get; set; }
        
    }

    public class DAVHeaderItems
    {
        
        public string CalendarAccess = "calendar-access"; //ICalendarItem
        public string AccessControl = "access-control"; //IAccessControl
        public string CalendarServerSharing = "calendarserver-sharing"; //IAppleCalendarAsync
        //https://tools.ietf.org/html/rfc6638
        public string CalendarAutoSchedule = "calendar-auto-schedule"; //ISchedulingPrincipalAsync, IScheduleInboxFolderAsync, IScheduleOutboxFolderAsync
        public string Paging = "paging";
        public string DAVLevel1 = "1";
        public string DAVLevel2 = "2";
        public string DAVLevel3 = "3";
        // public string Addressbook = "addressbook";
    }
}
