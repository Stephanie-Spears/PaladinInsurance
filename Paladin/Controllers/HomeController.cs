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
 *
 */

/* Extension Point 6
 * Extension Point: Validation
 * Benefit: Simplify cross-property and conditional validation
 * (Extending Validation to Improve Data Integrity)
 * Summary:
 *
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


