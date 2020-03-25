using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.Web.Util;

namespace TheBookshelf.Web
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// внедрение зависимостей
			NinjectModule tagmodule = new TagModule();
			NinjectModule serviceModule = new ServiceModule("BookshelfContext");
			var kernel = new StandardKernel(tagmodule, serviceModule);
			DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

		}
	}
}
