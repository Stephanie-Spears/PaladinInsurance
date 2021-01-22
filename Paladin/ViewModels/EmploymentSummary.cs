using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Paladin.ViewModels
{
    public class EmploymentSummary : IValidatableObject
    {
	    public EmploymentSummary()
	    {
		    PrimaryEmployer = new EmploymentVM();
		    PreviousEmployer = new EmploymentVM();
	    }

		public EmploymentVM PrimaryEmployer { get; set; }
		public EmploymentVM PreviousEmployer { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();
			if (PrimaryEmployer.StartDate > DateTime.Now.AddYears(-3))
			{
				if (PreviousEmployer.EmploymentType != "Unemployed")
				{
					if (string.IsNullOrEmpty(PreviousEmployer.Employer))
					{
						results.Add(new ValidationResult("Previous employer is required."));
					}

					if (string.IsNullOrEmpty(PreviousEmployer.Position))
					{
						results.Add(new ValidationResult("Previous position is required."));
					}
				}
			}

			return results;
		}
    }
}
