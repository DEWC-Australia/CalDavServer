using CalDav.CalendarObjectRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CalDav.Models.Requests
{
    public abstract class CalDavRequest
    {
       
        public abstract ActionResult Process();

        protected static string ToString(ICalendarObject obj)
        {
            var calendar = new Calendar();
            calendar.AddItem(obj);

            var serializer = new Serializer();

            using (var str = new System.IO.StringWriter())
            {
                serializer.Serialize(str, calendar);
                return str.ToString();
            }
        }

    }
}
