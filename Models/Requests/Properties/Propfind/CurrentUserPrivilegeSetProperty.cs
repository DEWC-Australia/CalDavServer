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
    class CurrentUserPrivilegeSetProperty : PropfindProperty
    {
        public override string Key => "current-user-privilege-set";

        public override XName Name => Common.xDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {

            var privilegeName = Common.xDav.GetName("privilege");

            var currentUserPrivilegeSet = !allprop && props.All(x => x.Name != this.Name)
                    ? null
                    : this.Name.Element();

            supportedProperties.Add(this.Name);

            if (currentUserPrivilegeSet != null)
            {
                if (CalendarRepository.Read)
                    currentUserPrivilegeSet.Add(privilegeName.Element(Common.xDav.Element("read")));

                if (CalendarRepository.Write)
                    currentUserPrivilegeSet.Add(privilegeName.Element(Common.xDav.Element("write")));
            }

            return currentUserPrivilegeSet;
        }
    }
}
