using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Paladin.Infrastructure
{
    public class HttpValueProvider : IValueProvider
    {
	    private readonly NameValueCollection _headers;
	    private readonly string[] _headerKeys;


		// We're passing in a NameValueCollection which is the ObjectType of the headers on the Request object. 
	    public HttpValueProvider(NameValueCollection httpHeaders)
	    {
			// We assign two fields, one to store this full header collection, and one to store only the keys of the collection (not the values).
		    _headers = httpHeaders;
		    _headerKeys = _headers.AllKeys;
	    }


		// This prefix parameter gets passed in by the ModelBinder, and it's a string name of the property that it's currently trying to bind. All we have to do is return a true or false for this method depending on whether our collection of headers contains a match for that property name. However, many Http Header names include a hyphen in them, but the property names passed in from our models do not. So we remove the hyphens using a Lambda expression. We also ignore case here.
	    public bool ContainsPrefix(string prefix)
	    {
		    return _headerKeys.Any(h => h.Replace("-", "").Equals(prefix, StringComparison.OrdinalIgnoreCase));
	    }


		// This is similar to the above method, except that we're retrieving the value instead of just running a check.
	    public ValueProviderResult GetValue(string key)
	    {
			// We're storing the matching header key if we find one, instead of just checking if it exists. 
		    var header =
			    _headerKeys.FirstOrDefault(h => h.Replace("-", "").Equals(key, StringComparison.OrdinalIgnoreCase));

			// This conditional checks to see if the value associated with that key in the FullNameValueCollection is null. If not, we return a new ValueProviderResult with that retrieved value. 
		    if (!string.IsNullOrEmpty(_headers[header]))
		    {
			    return new ValueProviderResult(_headers[header], _headers[header], CultureInfo.CurrentCulture);
		    }

		    return null;
	    }
    }

    public class HttpValueProviderFactory : ValueProviderFactory
    {
	    public override IValueProvider GetValueProvider(ControllerContext controllerContext)
	    {
		    return new HttpValueProvider(controllerContext.HttpContext.Request.Headers);
	    }
    }
}
