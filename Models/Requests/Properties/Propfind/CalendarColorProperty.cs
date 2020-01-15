using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;

namespace CalDav.Models.Requests.Properties.Propfind
{
    class CalendarColorProperty : PropfindProperty
    {
        public override string Key => "calendar-color";

        public override XName Name => Common.xApple.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {

            var calendarColor = !allprop && props.All(x => x.Name != Name)
                    ? null
                    : Name.Element(CalendarRepository.CalendarColor);

            supportedProperties.Add(Name);

            return calendarColor;
        }
    }
}
