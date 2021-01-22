using AutoMapper;
using Paladin.Infrastructure;
using Paladin.Models;
using Paladin.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Paladin
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			// First we are grabbing the value of our active theme from the Web.config settings
			if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActiveTheme"]))
			{
				// If it isn't empty, then we want it to register our ThemeViewEngine and pass in the name of that active theme.
				var activeTheme = ConfigurationManager.AppSettings["ActiveTheme"];

				// Notice that we're using the Insert method to add our ViewEngine to the collection, not the Add method. MVC can have multiple ViewEngines registered at the same time, but if we use the Insert method, that allows us to specify an index, which we can set to 0, and that means that our ViewEngine will be evaluated first.
				ViewEngines.Engines.Insert(0, new ThemeViewEngine(activeTheme));
			};


			// When implementing a custom ModelBinderProvider we have to register it with our application. By using Insert and specifying the 0 index, we can be sure that our binder will be evaluated first. This way for XML requests, our binder will always be chosen, and if it's not an XML request, it will never be chosen. 
			ModelBinderProviders.BinderProviders.Insert(0, new XMLModelBinderProvider());

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			Mapper.CreateMap<ApplicantVM, Applicant>();
			Mapper.CreateMap<VehicleVM, Vehicle>();
			Mapper.CreateMap<AddressVM, Address>();
			Mapper.CreateMap<EmploymentVM, Employment>();
			Mapper.CreateMap<ProductsVM, Products>();

			Mapper.CreateMap<Applicant, ApplicantVM>();
			Mapper.CreateMap<Vehicle, VehicleVM>();
			Mapper.CreateMap<Address, AddressVM>();
			Mapper.CreateMap<Employment, EmploymentVM>();
			Mapper.CreateMap<Products, ProductsVM>();
		}


		// Global Application Error Handler Method
		void Application_Error()
		{
			Debug.WriteLine("test");
		}
	}
}