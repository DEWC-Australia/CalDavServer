using System;
using CalDav.Data.CALDAV;
using CalDav.Models.FileSystem;
using CalDav.Models.FileSystem.Folder;
using CalDav.Models.RequestStrategies;
using CalDav.Models.RequestStrategies.Propfind;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class Propfind : CalDavRequest
    {
        private CalDavRequestData RequestData { get; set; }
        private AbstractFolder Folder { get; set; }
        private StrategyContext StratContext { get; set; }

        public Propfind(CalDavRequestData requestData, AbstractFolder folder)
        {
            RequestData = requestData;
            Folder = folder;
        }

        public override ActionResult Process()
        {

            if (Folder.Type == FileSystem.FolderType.WellKnown)
            {
                StratContext = new StrategyContext(new WellKnownStrategy(Folder));
            }
            else
            {
                StratContext = new StrategyContext(new GeneralStrategy(RequestData, Folder));
            }


            return StratContext.executeStrategy();
        }

    }


}
