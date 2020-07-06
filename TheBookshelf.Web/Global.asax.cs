using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
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

			GlobalConfiguration.Configure(WebApiConfig.DependencyInject);
		}
		protected void Application_BeginRequest()
		{
			if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
			{
				Response.Flush();
			}
		}
	}
}
