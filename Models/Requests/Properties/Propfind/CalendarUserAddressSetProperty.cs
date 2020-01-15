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
    class CalendarUserAddressSetProperty : PropfindProperty
    {
        public override string Key => "calendar-user-address-set";

        public override XName Name => Common.xCalDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder, FolderType.AclFolder, FolderType.CalendarHomeset, FolderType.ContextPath };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {
            var hrefName = Common.xDav.GetName("href");

            var calendarUserAddress = !allprop && props.All(x => x.Name != Name)
                ? null
                : Name.Element(hrefName.Element(RequestData.CurrentPrincipal.Path),
                    hrefName.Element("mailto:" + RequestData.CurrentPrincipal.CurrentUser.UserName));

            supportedProperties.Add(Name);

            return calendarUserAddress;
        }
    }
}
