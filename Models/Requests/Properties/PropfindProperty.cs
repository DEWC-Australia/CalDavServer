using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;

namespace CalDav.Models.Requests.Properties
{
    public abstract class PropfindProperty : IProperty
    {
        public abstract string Key { get; }

        public abstract XName Name { get; }

        public abstract List<FolderType> SupportedTypes {get;}

        public AbstractCalendarRepository CalendarRepository { get; protected set; }

        protected CalDavRequestData RequestData { get; set; }

        public XElement Build(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties, CalDavRequestData requestData)
        {
            RequestData = requestData;

            if (!FolderSupported(currentFolder))
                return null;

            IsCalendarRepository(currentFolder);

            var result = Do(allprop, props, currentFolder, elementBase, supportedProperties);

            AddToBase(elementBase, result);

            return result;
        }

        protected abstract XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties);
        protected void AddToBase(XElement elementBase, XElement element)
        {
            if (element != null)
                elementBase.Add(element);
        }

        protected bool FolderSupported(AbstractFolder currentFolder)
        {
            foreach(var type in this.SupportedTypes ?? new List<FolderType>())
            {
                if (currentFolder.Type == type)
                    return true;

            }

            return false;
        }

        protected void IsCalendarRepository(AbstractFolder currentFolder)
        {
            if (currentFolder.Type == FileSystem.FolderType.CalendarFolder)
                this.CalendarRepository = (AbstractCalendarRepository)currentFolder;
        }

    }
}
