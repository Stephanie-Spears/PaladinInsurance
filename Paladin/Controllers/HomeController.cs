using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paladin.Models;

namespace Paladin.Controllers
{
    [OverrideAuthorization]
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
            if (Session["Tracker"] != null)
            {
                Guid tracker = (Guid)Session["Tracker"];
                var highestStage = _context.Applicants.FirstOrDefault(x => x.ApplicantTracker == tracker).WorkFlowStage;
                return PartialView(new Progress { CurrentStage = currentStage, HighestStage = highestStage });
            }
            else
            {
                return PartialView(new Progress { CurrentStage = 10, HighestStage = 10 });
            }
            return PartialView();
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
 */

/* Extension Point 4
 * Extension Point: Razor View Engine
 * Benefit: Theme support, custom directory naming
 * (Enabling Theme Support with a Custom View Engine)
 */

/* Extension Point 5
 * Extension Point: Exception Filters
 * Benefit: Simplify exception handling
 * (Improving Error Handling with Custom Exception Filters)
 */

/* Extension Point 6
 * Extension Point: Validation
 * Benefit: Simplify cross-property and conditional validation
 * (Extending Validation to Improve Data Integrity)
 */

/* Extension Point 7
 * Extension Point: Model Binding
 * Benefit: Expand the binding process to handle new data types
 * (Extending Data Binding with Custom Model Binders)
 */

/* Extension Point 8
 * Extension Point: Value Providers
 * Benefit: Limit or increase the data available for Model Binding
 * (Improving Data Availability with Custom Value Providers)
 */

/* Extension Point 9
 * Extension Point: Authentication Filters
 * Benefit: Customize access control
 * (Customizing Security Using Authentication Filters)
 */

/* Extension Point 10
 * Extension Point: Action Selectors
 * Benefit: Influence the Action Method Selection Process
 * (Influencing Action Method Execution Using Custom Selectors)
 */


