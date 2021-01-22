using System;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Paladin.Infrastructure
{
    public class XMLModelBinder : IModelBinder
    {
	    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
	    {
		    try
		    {
				// Grab the current ModelType that the framework is trying to bind off of this bindingContext object. This works nicely with the XmlSerializer just below that, which expects us to provide a ModelType to deserialize to. 
			    var modelType = bindingContext.ModelType;
			    var serializer = new XmlSerializer(modelType);


				// Here we are simply consuming the HttpContext InputStream and returning the object.
			    var inputStream = controllerContext.HttpContext.Request.InputStream;

			    return serializer.Deserialize(inputStream);
		    }
		    catch (Exception ex)
		    {
				// Here we are adding some really basic model validation to this Model Binder. We can access the ModelState through our bindingContext property, which allows us to add errors that we can check inside of our action methods. In this case, we're just returning a really simple message stating that serialization failed.
			    bindingContext.ModelState.AddModelError("", "The item could not be serialized");

				// We return null here if there is an error, since we'll handle the rest of the response in our action methods. 
			    return null;
		    }
	    }
    }
}
