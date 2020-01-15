using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;

namespace CalDav.Models.Requests.Properties.Propfind
{
    class SupportedReportSetProperty : PropfindProperty
    {
        public override string Key => "supported-report-set";

        public override XName Name => Common.xDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {

            List<XElement> supportedReportSet = new List<XElement>();

            if (!(!allprop && props.All(x => x.Name != Name)))
            {
                supportedProperties.Add(Name);

                if (CalendarRepository.AclPrincipalPropSet)
                {
                    supportedReportSet.Add(Name.Element(Common.xDav.Element("supported-report",
                    Common.xDav.Element("report", Common.xDav.Element("acl-principal-prop-set")))));
                }

                if (CalendarRepository.PrincipalMatch)
                {
                    supportedReportSet.Add(Name.Element(Common.xDav.Element("supported-report",
                    Common.xDav.Element("report", Common.xDav.Element("principal-match")))));
                }

                if (CalendarRepository.PrincipalPropertySearch)
                {
                    supportedReportSet.Add(Name.Element(Common.xDav.Element("supported-report",
                    Common.xDav.Element("report", Common.xDav.Element("principal-property-search")))));
                }

                if (CalendarRepository.CalendarMultiGet)
                {
                    supportedReportSet.Add(Name.Element(Common.xDav.Element("supported-report",
                    Common.xDav.Element("report", Common.xCalDav.Element("calendar-multiget")))));
                }

                if (CalendarRepository.CalendarQuery)
                {
                    supportedReportSet.Add(Name.Element(Common.xDav.Element("supported-report",
                    Common.xDav.Element("report", Common.xCalDav.Element("calendar-query")))));
                }

                if (supportedReportSet.Count() > 0)
                    elementBase.Add(supportedReportSet);

            }

            return null;

        }
    }
}
