using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalDav.Attributes
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthorizeAttribute : TypeFilterAttribute
    {
        public BasicAuthorizeAttribute(string realm = null)
            : base(typeof(BasicAuthorizeFilter))
        {
            Arguments = new object[]
            {
                realm
            };
        }
    }
}
