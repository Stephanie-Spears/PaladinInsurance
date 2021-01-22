using System;
using System.ComponentModel.DataAnnotations;

namespace Paladin.ViewModels
{
	public class EmploymentVM
	{
		// The required data attribute has to be removed because even though the PrimaryEmployment section requires it, the PreviousEmployment section does not, and these attributes would apply to both. 
		//[Required]
		[Display(Name = "Employment Type")]
		public string EmploymentType { get; set; }
		//[Required]
		public string Employer { get; set; }
		//[Required]
		public string Position { get; set; }
		//[Required]
		[Display(Name = "Gross Monthly Income")]
		public double GrossMonthlyIncome { get; set; }
		//[Required]
		[Display(Name = "Start Date")]
		public DateTime? StartDate { get; set; }
	}
}