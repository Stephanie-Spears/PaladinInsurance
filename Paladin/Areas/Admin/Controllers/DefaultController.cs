using System.Web.Mvc;

namespace Paladin.Areas.Admin.Controllers
{
	public class DefaultController : Controller
	{
		// GET: Admin/Default
		public ActionResult Index()
		{
			return View();
		}
	}
}