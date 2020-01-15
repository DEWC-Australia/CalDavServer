using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.Extensions.Primitives;

namespace CalDav.Models
{
    public class Result : ActionResult
    {
        public System.Net.HttpStatusCode? Status { get; set; }
        public object Content { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string ContentType { get; set; }

        public string DavHeader { get; set; }

        public Result()
        {
            
        }

        public Result(Action<ControllerContext> content)
        {
            Content = content;
        }

        public Result(byte[] content)
        {
            Content = content;
        }


        public override void ExecuteResult(ActionContext context)
        {
            var res = context.HttpContext.Response;
            res.StatusCode = (int)(Status ?? System.Net.HttpStatusCode.OK);

            if (Headers != null && Headers.Count > 0)
            {
                foreach (var header in Headers)
                {
                    res.Headers.Add(header.Key, header.Value);
                }
            }
            res.Headers.Add(CalDavSettings.HttpHeader.XENVVERSION, "ASP.NET Core 2.1");
            res.Headers.Add(CalDavSettings.HttpHeader.XOSVERSION, System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            res.Headers.Add(CalDavSettings.HttpHeader.XENGINE, "CalDav-Server V0.1");
            // res.Headers.Add("DAV", "1, 2, 3, calendar-access, calendar-schedule, calendar-proxy");
            if(DavHeader != null)
                res.Headers.Add(CalDavSettings.HttpHeader.DAV, DavHeader);

            var content = Content;

            if (content is XElement)
            {
                ContentType = CalDavSettings.HttpContentType.TEXTXML;
            }

            if (ContentType != null)
            {

                if (res.Headers.ContainsKey(CalDavSettings.HttpHeader.CONTENTTYPE))
                {
                    res.Headers.Remove(CalDavSettings.HttpHeader.CONTENTTYPE);
                }

                string[] values = { ContentType, CalDavSettings.HttpContentType.CHARSETUTF8 };
                res.Headers.Add(CalDavSettings.HttpHeader.CONTENTTYPE, new StringValues(values));
            }


            if (content is XDocument)
            {
                content = ((XDocument)content).Root;
            }


            if (content is XElement)
            {
                
                ((XElement)content).Save(res.Body);
            }
            else
            { 
                if (content is string)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(content as string);
                    res.Body.Write(data, 0, data.Length);
                }
                else
                {
                    if (content is byte[])
                    {
                        byte[] data = content as byte[];
                        res.Body.Write(data, 0, data.Length);
                    }
                    else
                    {
                        if (content is Action<ActionContext>)
                        {
                            ((Action<ActionContext>)content)(context);
                        }
                    }
                }
                
            }
           
           
        }

    }
}