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
    class PrincipalUrlProperty : PropfindProperty
    {
        public override string Key => "principal-URL";

        public override XName Name => Common.xDav.GetName(Key);
        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder, FolderType.AclFolder, FolderType.CalendarHomeset, FolderType.ContextPath };
        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {
            var hrefName = Common.xDav.GetName("href");
            var principalUrl = !allprop && props.All(x => x.Name != Name)
                ? null
                : Name.Element(hrefName.Element(RequestData.CurrentPrincipal.PrincipalURL));

            supportedProperties.Add(Name);

            return principalUrl;
        }
    }
}
