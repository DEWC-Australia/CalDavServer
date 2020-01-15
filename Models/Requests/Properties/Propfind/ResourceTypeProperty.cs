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
    class ResourceTypeProperty : PropfindProperty
    {
        public override string Key => "resourcetype";

        public override XName Name => Common.xDav.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder, FolderType.AclFolder, FolderType.CalendarHomeset, FolderType.ContextPath };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {
            XElement resourceType = null;
            if (!allprop && props.All(x => x.Name != Name))
            {
                resourceType = null;
            }
            else if (currentFolder.Type == FileSystem.FolderType.AclFolder)
                resourceType = Name.Element(Common.xDav.Element("collection"), Common.xDav.Element("principal"));

            else if (currentFolder.Type == FileSystem.FolderType.ContextPath)
                resourceType = Name.Element(Common.xDav.Element("collection"));

            else if (currentFolder.Type == FileSystem.FolderType.CalendarHomeset)
                resourceType = Name.Element(Common.xDav.Element("collection"));

            else if (currentFolder.Type == FileSystem.FolderType.CalendarFolder && CalendarRepository.SharedOwner)
                resourceType = Name.Element(Common.xDav.Element("collection"), Common.xCalDav.Element("calendar"), Common.xCalDav.Element("shared-owner"));

            else if (currentFolder.Type == FileSystem.FolderType.CalendarFolder)
                resourceType = Name.Element(Common.xDav.Element("collection"), Common.xCalDav.Element("calendar"));

            supportedProperties.Add(Name);
 
            return resourceType;
        }
    }
}
