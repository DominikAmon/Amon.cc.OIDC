using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using Amon.cc.Oidc.SampleApplication.Handler;
using System.Configuration;

namespace Amon.cc.Oidc.SampleApplication
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			RouteTable.Routes.Add(new Route(ConfigurationManager.AppSettings["authenticationCallbackUrl"], new GenericRouteHandler<OAuth2CallbackHandler>()));
		}		
	
	}
}