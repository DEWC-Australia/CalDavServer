﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CalDav.CalendarObjectRepository;
using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;

namespace CalDav.Models.Requests.Properties.Propfind
{
    class GetCtagProperty : PropfindProperty
    {
        public override string Key => "getctag";

        public override XName Name => Common.xCalServer.GetName(Key);

        public override List<FolderType> SupportedTypes => new List<FolderType> { FolderType.CalendarFolder };

        protected override XElement Do(bool allprop, List<XElement> props, AbstractFolder currentFolder, XElement elementBase, HashSet<XName> supportedProperties)
        {

            var getctag = !allprop && props.All(x => x.Name != Name)
                    ? null
                    : Name.Element(CalendarRepository.Ctag);

            supportedProperties.Add(Name);

            return getctag;
        }
    }
}
