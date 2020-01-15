using CalDav.Models.FileSystem.Folder;
using CalDav.Models.Requests;
using System;
using System.Collections.Generic;
using System.IO;

namespace CalDav.Models.RequestStrategies.Propfind
{
    public class WellKnownStrategy: IStrategy
    {
        private AbstractFolder Folder { get; set; }

        public WellKnownStrategy(AbstractFolder folder)
        {
            Folder = folder;
        }
        protected string CleanPath(string path)
        {
            return path.Replace("/", "\\");
        }
        public Result DoOperation()
        {
            
            var wellKnownFolder = (WellKnownFolder)Folder;

            string directory = Path.Combine(Directory.GetCurrentDirectory(), CalDavSettings.SERVERFILEPATH + "\\" + CleanPath(wellKnownFolder.ContextPath));
            // ensure the context directory exists
            if (!Directory.Exists(directory))
            {
				Directory.CreateDirectory(directory);
				/*
				return new Result
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Content = new String("Context Path does not exist")
                };*/
            }
                

            // .well-known results in a redirect to the Contect Path
            return new Result {
                Status = System.Net.HttpStatusCode.MovedPermanently,
                Headers = new Dictionary<string, string> {
                    { CalDavSettings.HttpHeader.LOCATION, CalDavSettings.RouteSettings.CONTEXTPATH }
                }
            };
            
        }
    }
}
