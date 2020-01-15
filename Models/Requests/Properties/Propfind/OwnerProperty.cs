using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CalDav.Models.Requests.Properties.Propfind
{
    class OwnerProperty : PropfindProperty
    {
        
        public override string Key => "owner";

        public override XName Name => Common.xDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {

            var hrefName = Common.xDav.GetName("href");

            var owner = !allprop && props.All(x => x.Name != Name)
                ? null
                : Name.Element();

            supportedProperties.Add(Name);

            if (owner != null)
            {
                foreach (var ownerUrl in CalendarRepository.OwnerNames)
                {
                    owner.Add(hrefName.Element(ownerUrl));
                }
                
            }

            
            return owner;
        }

    }
}
