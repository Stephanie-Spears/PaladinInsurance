using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paladin.Models;
using Paladin.Infrastructure;

namespace Paladin.Controllers
{
	[ExceptionLoggingFilter]
	public class HomeController : Controller
	{
		private PaladinDbContext _context;

		public HomeController(PaladinDbContext context)
		{
			_context = context;
		}

		public ActionResult Index()
		{
			ViewBag.Home = "true";
			return View();
		}

		public ActionResult Error()
		{
			var test = int.Parse("test");
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult Final()
		{
			Session.Clear();
			return View();
		}

		public ActionResult Clear()
		{
			Session.Clear();
			return View();
		}

		public ActionResult ProgressBar(int currentStage)
		{
			if (Session["Tracker"] != null) //Have they started the workflow?
			{
				Guid tracker;
				if(Guid.TryParse(Session["Tracker"].ToString(), out tracker))
				{
					var highestStage = _context.Applicants.FirstOrDefault(x => x.ApplicantTracker == tracker).WorkFlowStage;
					return PartialView(new Progress { CurrentStage = currentStage, HighestStage = highestStage });
				}
                
			}
			//If not, show the first page
			return PartialView(new Progress { CurrentStage = 10, HighestStage = 10 });
		}
	}
}


/* Extension Point 1
 * Extension Point: Action Results
 * Benefit: Add efficiency to response handling, improve Action Method code
 * (Improving Application Responses with Custom Action Results)
 */

/* Extension Point 2
 * Extension Point: Action Filters
 * Benefit: Remove code repetition, separation of concerns, control application flow
 * (Organizing Application Flow with Action Filters)
 * Summary:
 * Action Filters can be used to inject logic into the MVC pipeline
 * Implement the IActionFilter interface and execute right before and after Action Methods
 * Expose a useful context object for working with requests
 * Can be applied at method, control, or global scope
 */

/* Extension Point 3
 * Extension Point: HTML Helpers
 * Benefit: Create cleaner and simpler Razor views
 * (Keeping Your Razor Code Clean with HTML Helpers)
 * Summary:
 * MVC provides multiple options for writing cleaner Razor code
 * Inline Razor Helpers use friendly syntax and reduce code, but are not very reusable
 * Custom Html Helpers can include C# logic and can be reused across multiple views
 * Html Helper code should be mostly limited to display logic
 */

/* Extension Point 4
 * Extension Point: Razor View Engine
 * Benefit: Theme support, custom directory naming
 * (Enabling Theme Support with a Custom View Engine)
 * Summary:
 * Use a custom View Engine to extend view selection functionality
 * Themes provide a dynamic way to manage different design templates
 * You can change where and how Razor selects a template file by modifying the View Engine's search locations
 * MVC supports multiple View Engines
 */

/* Extension Point 5
 * Extension Point: Exception Filters
 * Benefit: Simplify exception handling
 * (Improving Error Handling with Custom Exception Filters)
 * Summary:
 * Exception Filters provide flexibility for error handling
 * Can handle errors in the scope of Action Method execution - these filters can handle errors within the scope of Action Filters, Methods, and Action Results. 
 * Provide access to more contextual framework information - they execute within the context of MVC, and not at the main application level, which gives them access to additional useful information
 * Global error handling should still be used as a fallback - because they cannot handle errors that occur outside of the controllers, you should still have an additional fallback plan for other types of exceptions
 */

/* Extension Point 6
 * Extension Point: Validation
 * Benefit: Simplify cross-property and conditional validation
 * (Extending Validation to Improve Data Integrity)
 * Summary:
 * There are two methods of applying validation logic in MVC. The first is by using data attributes which can be applied to class properties to enforce certain rules or values. These attributes will automatically be consumed and applied by the framework. Default Validation Attributes: [Required] [Range] [StringLength] [Regular Expression] [Not Null], etc. 
 * The second method of applying validation logic in MVC is to implement an interface on your model which is called IValidatableObject. This interface allows you to add logic to your model to determine whether it contains valid data. This interface exposes a method called "Validate" which offers a lot of power. The method returns a list of validation results that will be consumed by MVC, so you can write as many different rules and conditionals inside of it as you want.
 * Both of these techniques have pros and cons:
 * The main benefit of using attributes is that they're reusable across multiple properties and different models, and they keep logic out of your models and controllers. They also allow for inheritance from existing validation attributes, which can be useful and save some time. The con is that the cross-property validation can be annoying to read and write, especially when you're working with ViewModels that might contain several complex nested properties. This might require you to use techniques like reflection to find various properties on other objects, which can become difficult to read and keep track of.
 * The main advantage of using the interface is that it provides an easier solution to the cross-property validation issue. However, this technique implies that your model now contain logic, which is usually not ideal. The interface also does not lend itself that well to reusability.
 * By default model validation is handled during the model binding process. When MVC goes to bind request data to your classes, it runs the applied validation attributes to check if the data is correct. If the model implements the IValidatableObject interface, this will be called afterward, but only if all of the attributes return valid. Essentially it's making sure that the properties are each individually valid before it tries to do cross-property or model-level validation. The combined results of this process are stored in the controller's ModelState property.
 */

/* Extension Point 7
 * Extension Point: Model Binding
 * Benefit: Expand the binding process to handle new data types
 * (Extending Data Binding with Custom Model Binders)
 * Summary:
 * 
 */

/* Extension Point 8
 * Extension Point: Value Providers
 * Benefit: Limit or increase the data available for Model Binding
 * (Improving Data Availability with Custom Value Providers)
 * Summary:
 * 
 */

/* Extension Point 9
 * Extension Point: Authentication Filters
 * Benefit: Customize access control
 * (Customizing Security Using Authentication Filters)
 * Summary:
 * 
 */

/* Extension Point 10
 * Extension Point: Action Selectors
 * Benefit: Influence the Action Method Selection Process
 * (Influencing Action Method Execution Using Custom Selectors)
 * Summary:
 * 
 */


