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
    class CalendarHomeSetProperty : PropfindProperty
    {
        public override string Key => "calendar-home-set";

        public override XName Name => Common.xCalDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.AclFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {
            var hrefName = Common.xDav.GetName("href");
            var calendarHomeSet = !allprop && props.All(x => x.Name != Name)
                    ? null
                    : Name.Element(hrefName.Element(RequestData.CurrentPrincipal.CalendarHomeSet));

            supportedProperties.Add(Name);

            return calendarHomeSet;
        }
    }
}
