using System.Web.Mvc;
using System.Xml.Serialization;
using Paladin.Models;

namespace Paladin.Controllers
{
    public class EMarketingController : Controller
    {
	    private PaladinDbContext _context;

	    public EMarketingController(PaladinDbContext context)
	    {
		    _context = context;
	    }

		/*
		 * Inside these two sample Action Methods we're manually instantiating an XmlSerializer and consuming the HttpRequest InputStream.
		 * The default model binder doesn't understand how to bind XML to a strongly-typed object so it can't provide this information to us as an action method parameter.
		 *
		 * Model Binding is the process of mapping data to action method parameters. This occurs after authorization and authentication filters have run, but right before action filters. This component is also responsible for data validation.
		 * Model binders usually retrieve the values they need for populating action parameters from value providers (though this is not required).
		 * MVC comes with several value providers out of the box, and these supply data from common sources that you'll use, like posted form values. Value Providers are a powerful extension point of their own.
		 * Model Binders are implemented through the IModelBinder interface, which exposes one method called BindModel.
		 * MVC 3 introduced another component to the binding process, called the ModelBinderProvider. This feature is also implemented through a simple interface, called IModelBinderProvider. The interface defines one method called GetBinder, which tries to supply an appropriate ModelBinder for the request, and will otherwise return null.
		 *
		 * After an Http Request is received by our application, and a controller is created, it's the job of the Action Invoker to determine which Action Method should handle the request. Once the Action Invoker selects a method, it then tries to locate a model binder to populate the parameters of the method. It does this by first checking if the model has a specific binder that's explicitly associated with is (such as through an attribute). If not, it loops through the ModelBinderProviders that are registered with the app and asks each of them if they can supply an appropriate binder for the given model. As soon as a model binder is provided, the remaining providers are ignored and no longer matter for that request. The supplied model binder is then used to populate the action method parameters. If there are no custom model binders registered, or none of them can supply a binder for the request, the default model binder is always used as a fallback.
		 *
		 * For this application we want to build a custom model binder that will handle mapping XML to our action methods using these concepts.
		 */

		// Our Action Methods now accept complex model objects to bind to.
		public ActionResult WeeklyReport(EWeeklyReport weeklyReport)
		{
			// we check model state here to make sure there are no serialization errors before we try to persist it to the database. 
			if (ModelState.IsValid)
			{
				_context.WeeklyReports.Add(weeklyReport);
				_context.SaveChanges();
				return Content("Ok");
			}

			return Content("Error");
		}

		public ActionResult MonthlyReport(EMonthlyReport monthlyReport)
	    {
		    if (ModelState.IsValid)
		    {
			    _context.MonthlyReports.Add(monthlyReport);
			    _context.SaveChanges();
			    return Content("OK");
		    }

		    return Content("Error");
	    }
    }
}