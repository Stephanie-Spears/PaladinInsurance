using System;
using System.Web;
using System.Web.Mvc;

namespace Paladin.Infrastructure
{
    public class XMLModelBinderProvider : IModelBinderProvider
    {
	    public IModelBinder GetBinder(Type modelType)
	    {
			// Check if the current Http Request ContentType header is set to "text/xml", since we only want this binder applied to XML requests.
		    var contentType = HttpContext.Current.Request.ContentType.ToLower();
		    if (contentType != "text/xml")
		    {
				// if the header isn't set to "text/xml" we return null, which will prompt the framework to move onto the next ModelBinder option
			    return null;
		    }

			// if the header is an XML request, then we return a new instance of our XMLModelBinder. This class will then be used by the framework to try and bind our action method parameters for these types of requests. 
		    return new XMLModelBinder();
	    }
    }
}
