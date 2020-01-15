using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CalDav.CalendarObjectRepository
{
	public abstract class ObjectBase
	{
		public void ValidateModel()
		{
			var context = new ValidationContext(this, serviceProvider: null, items: null);
			var results = new List<ValidationResult>();

			Validator.TryValidateObject(this, context, results);

			if (!Validator.TryValidateObject(this, context, results))
			{
				StringBuilder exMsg = new StringBuilder();

				foreach (var error in results)
				{
					exMsg.AppendLine(error.ErrorMessage);
				}

				throw new ValidationException(exMsg.ToString());
			}

		}
	}
}
