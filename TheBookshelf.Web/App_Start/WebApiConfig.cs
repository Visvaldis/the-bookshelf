using Microsoft.Owin.Security.OAuth;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.Web.Util;

namespace TheBookshelf.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Конфигурация и службы веб-API

			config.SuppressDefaultHostAuthentication();
			config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
			// Маршруты веб-API
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}

		public static StandardKernel Kernel { get; private set; }

		internal static void DependencyInject(HttpConfiguration config)
		{
			INinjectModule[] RegisterModules()
			{
				return new INinjectModule[]
				{
					new UserModule(),
					new TagModule(),
					new ServiceModule("BookshelfContext")
				};
			}

		//	StandardKernel
			Kernel = new StandardKernel(RegisterModules());
			config.DependencyResolver = new NinjectResolver(Kernel);
		}

	}
}
