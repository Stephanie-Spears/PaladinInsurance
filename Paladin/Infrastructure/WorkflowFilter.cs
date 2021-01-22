using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Paladin.Models;

/*
 * Extension Point 2
 * Extension Point: Action Filters
 * Benefit: Remove code repetition, separation of concerns, control application flow
 * (Organizing Application Flow with Action Filters)
 * Summary:
 * Action Filters can be used to inject logic into the MVC pipeline
 * Implement the IActionFilter interface and execute right before and after Action Methods
 * Expose a useful context object for working with requests
 * Can be applied at method, control, or global scope
 
 WORKFLOW:
 * Request->OnActionExecuting[(Active Session ?) ->YES-> (Completed Minimum Required Stage ?) ] -> [Action Method] -> OnActionExecuted[(Update Database with Workflow Status)] => Request
 * Request -> OnActionExecuting [ (Active Session ?) -> YES -> (Completed Minimum Required Stage?) ] -> NO => Redirect to Last Completed Page
 * Request -> OnActionExecuting [ (Active Session ?)] -> NO => Redirect To First Page
 *
 * */


namespace Paladin.Infrastructure
{
	public class WorkflowFilter : FilterAttribute, IActionFilter
	{
		// stores the value we pull from the database which will tell us the highest completed workflow stage for the current user session
		private int _highestCompletedStage;
		// public properties on Action Filters can be easily set from our controllers, so we can define a unique value in our Action Filter for every stage in our workflow
		public int MinRequiredStage { get; set; }
		// this property will define the value of the workflow stage that the user is currently trying to access, so if they complete this page THIS is the new value that will be assigned to them in the database to mark their status in the workflow.
		public int CurrentStage { get; set; }



		/*
		 * Three main steps:
		 * The first issue we want to solve is to check whether the user has an active session(which is set in our Action Method the first time they save anything to the database for the "Get a Quote" form.
		 * Abstracting this code into our Action Filter will also allow us to remove the repetitive session checking code from our Action Methods (EmploymentController.cs, ProductsController.cs, VehicleController.cs, etc.)
		*/

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{

			// In the first "if" block we are simply checking if an ApplicantId is set in our Session tracker variable. 
			var applicantId = filterContext.HttpContext.Session["Tracker"];

			//	If a value is set, we want to make sure that it can be parsed into a guid.
			if (applicantId != null)
			{
				if (Guid.TryParse(applicantId.ToString(), out Guid tracker))
				{
					// The next thing that we want to enforce within our OnActionExecuting section is that the user has completed the minimum required workflow stage to get to this new current page that they're trying to access. To do this, we will need to check the database. 
					// All this is doing is retrieving the saved workflow value from the database for the current session, and then assigning it to the _highestCompletedStage field
					var _context = DependencyResolver.Current.GetService<PaladinDbContext>(); // The DependencyResolver line here simply retrieves our DbContext object using the Current resolver, just to make sure that we are getting the right instance of this class.
					_highestCompletedStage = _context.Applicants.FirstOrDefault(x => x.ApplicantTracker == tracker).WorkFlowStage;

					// If the MinRequiredStage is higher than the saved value, that means that they have not completed the pages necessary up until that point to get here, so we need to redirect them. 
					if (MinRequiredStage > _highestCompletedStage)
					{
						// To handle redirecting to the right page, we use this switch statement which just checks what the highest page the user has completed and redirects appropriately.
						// TODO: in real-world project this should be more dynamic, ie. customizing the routing engine to redirect to the right page based on the value
						switch (_highestCompletedStage)
						{
							case (int)WorkflowValues.ApplicantInfo:
								filterContext.Result = GenerateRedirectUrl("ApplicantInfo", "Applicant");
								break;

							case (int)WorkflowValues.AddressInfo:
								filterContext.Result = GenerateRedirectUrl("AddressInfo", "Address");
								break;

							case (int)WorkflowValues.EmploymentInfo:
								filterContext.Result = GenerateRedirectUrl("EmploymentInfo", "Employment");
								break;

							case (int)WorkflowValues.VehicleInfo:
								filterContext.Result = GenerateRedirectUrl("VehicleInfo", "Vehicle");
								break;

							case (int)WorkflowValues.ProductInfo:
								filterContext.Result = GenerateRedirectUrl("ProductInfo", "Products");
								break;
						}

					}
				}
			}
			// If a value is NOT set, we want to redirect the user back to the first page because this is probably implies that they didn't start the "Get a Quote" workflow correctly, or they're trying to skip ahead to a page that they shouldn't be able to access yet. 
			else
			{
				// We are also checking here that they aren't currently on the first page, otherwise we will get into a redirect loop which will cause problems in the browser. 
				if (CurrentStage != (int)WorkflowValues.ApplicantInfo)
				{
					// The most interesting line of code here is where we assign the RedirectToRouteResult property of this filterContext object. The Result property is actually an ActionResult type, so by assigning it here the final result of our Action Method will now be a redirect. This is where we can really take control of our application flow. 
					filterContext.Result = GenerateRedirectUrl("ApplicantInfo", "Applicant");
				}
			}
		}

		// The GenerateRedirectUrl method is mostly just a syntactical helper that will make our code more readable when we're doing these redirects. All it does is just return a new RedirectToRouteResult with the values that we passed in, so that we don't have to manually create this every time. 
		private RedirectToRouteResult GenerateRedirectUrl(string action, string controller)
		{
			return new RedirectToRouteResult(new RouteValueDictionary(new { action = action, controller = controller }));
		}


		/*
		 * We repeat some things from the custom OnActionExecuting method above because it is possible that the session variable, or even the context class, could change or be newly assigned in the logic of the Action Method, which runs between these two methods.
		 * For example, on the first page, the "tracker" variable does not exist yet in the OnActionExecuting method.Once it's assigned inside of the Action Method, it does exist by the time this OnActionExecuted method runs.
		 * */


		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var _context = DependencyResolver.Current.GetService<PaladinDbContext>();
			var sessionId = HttpContext.Current.Session["Tracker"];
			if (sessionId != null)
			{
				/*
				 * This code inside the TryParse condition is what updates the database with the user's new workflow status.
				 * We only want to do this when they're saving a new form, so first we check if the request is a post, since those are the requests that will send data back.
				 * We also want to make sure that the value we're saving is equal to or greater than the one that's already saved(because we don't want to save the current workflow value if the user is simply returning to an earlier page to change what they previously entered. We only want update the database with the highest stage that they have ever completed in this process. 
				*/
				if (Guid.TryParse(sessionId.ToString(), out Guid tracker))
				{
					if (filterContext.HttpContext.Request.RequestType == "POST" && CurrentStage >= _highestCompletedStage)
					{
						var applicant = _context.Applicants.FirstOrDefault(x => x.ApplicantTracker == tracker);
						applicant.WorkFlowStage = CurrentStage;
						_context.SaveChanges();
					}
				}
			}
		}
	}
}

/*
 * ActionFilters can be added at the Action Method level or at the Controller level, or even globally.
 */