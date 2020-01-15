using CalDav.Models.FileSystem.Folder;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CalDav.Models.Requests.Properties
{
    public interface IProperty
    {
        string Key { get; }
        XName Name { get; }

        XElement Build(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties, CalDavRequestData requestData);

    }
}
