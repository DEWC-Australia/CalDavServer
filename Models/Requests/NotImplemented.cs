using CalDav.Models.FileSystem.Folder;
using Microsoft.AspNetCore.Mvc;

namespace CalDav.Models.Requests
{
    class NotImplemented : CalDavRequest
    {
        public NotImplemented()
        {
        }

        public override ActionResult Process()
        {
            return new Result { Status = System.Net.HttpStatusCode.NotImplemented };
        }
    }

	class ForbiddenAccess : CalDavRequest
	{
		public ForbiddenAccess()
		{
		}

		public override ActionResult Process()
		{
			return new Result { Status = System.Net.HttpStatusCode.Forbidden };
		}
	}
}
