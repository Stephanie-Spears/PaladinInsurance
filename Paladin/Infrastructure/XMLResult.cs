using System.Web.Mvc;
using System.Xml.Serialization;

namespace Paladin.Infrastructure
{
	public class XMLResult : ActionResult
	{
		private object _data;
        
		public XMLResult(object data)
		{
			_data = data;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			XmlSerializer serializer = new XmlSerializer(_data.GetType());
			var response = context.HttpContext.Response;
			response.ContentType = "text/xml";
			serializer.Serialize(response.Output, _data);
		}
	}
}

/* Extension Point 1
 * Extension Point: Action Results
 * Benefit: Add efficiency to response handling, improve Action Method code
 * (Improving Application Responses with Custom Action Results)
 */