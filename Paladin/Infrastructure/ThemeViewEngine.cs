using System.Web.Mvc;

namespace Paladin.Infrastructure
{
	// inherits from RazorViewEngine
	public class ThemeViewEngine : RazorViewEngine
	{
		public ThemeViewEngine(string activeThemeName)
		{
			// Assign values to a few properties called "View Location Formats" and "Partial View Location Formats" that are exposed on the parent class. These are defined on the VirtualPathProvider ViewEngine parent class.
			// What we want to do is take the default locations used by Razor and simply add a theme directory to that path, as well as a variable for the active theme.
			// These are very similar to the default paths, except that we've added in these additional theme segments. 
			// This just takes the same paths and duplicates them for each property in the parent class. In this case we're also taking MVC areas into account down at the bottom "AreaViewLocationFormats = new[] and "AreaPartialViewLocationFormats = new[]"
			// The last thing that we have to do in this class is assign the activeThemeName variable, which will come from the Web.config. The easiest way to grab this variable will be to pass it in as a parameter to our constructor.
			ViewLocationFormats = new[]
			{
				"~/Views/Themes/" + activeThemeName + "/{1}/{0}.cshtml",
				"~/Views/Themes/" + activeThemeName + "/Shared/{0}.cshtml"
			};

			PartialViewLocationFormats = new[]
			{
				"~/Views/Themes/" + activeThemeName + "/{1}/{0}.cshtml",
				"~/Views/Themes/" + activeThemeName + "/Shared/{0}.cshtml"
			};

			AreaViewLocationFormats = new[]
			{
				"~Areas/{2}/Views/Themes/" + activeThemeName + "/{1}/{0}.cshtml",
				"~Areas/{2}/Views/Themes/" + activeThemeName + "/Shared/{0}.cshtml"
			};

			AreaPartialViewLocationFormats = new[]
			{
				"~Areas/{2}/Views/Themes/" + activeThemeName + "/{1}/{0}.cshtml",
				"~Areas/{2}/Views/Themes/" + activeThemeName + "/Shared/{0}.cshtml"
			};
		}
	}
}