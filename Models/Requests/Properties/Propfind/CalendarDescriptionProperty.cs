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
    class CalendarDescriptionProperty : PropfindProperty
    {
        public override string Key => "calendar-description";

        public override XName Name => Common.xCalDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {
            var calendarDescription = (currentFolder.Type == FileSystem.FolderType.CalendarFolder) ||
                                              (!allprop && props.All(x => x.Name != Name))
                        ? null
                        : Name.Element(CalendarRepository.CalendarDescription);

            supportedProperties.Add(Name);

            return calendarDescription;
        }
    }
}
