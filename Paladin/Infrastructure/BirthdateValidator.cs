using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Paladin.Infrastructure
{
	public class BirthdateValidator : ValidationAttribute
	{
		public BirthdateValidator()
		{
			ErrorMessage = "Please enter a valid birthdate. You must be 18 or older to apply.";
		}

		public override bool IsValid(object value)
		{
			if (DateTime.TryParse(value.ToString(), out DateTime enteredDate))
			{
				if (enteredDate > DateTime.Now.AddYears(-18))
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}
	}
}