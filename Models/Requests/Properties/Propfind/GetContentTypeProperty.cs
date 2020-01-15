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
    class GetContentTypeProperty : PropfindProperty
    {
        public override string Key => "getcontenttype";

        public override XName Name => Common.xDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {
            var getContentType = !allprop && props.All(x => x.Name != Name)
                    ? null
                    : Name.Element("text/calendar; component=vevent");


            supportedProperties.Add(Name);

            return getContentType;

        }
    }
}
