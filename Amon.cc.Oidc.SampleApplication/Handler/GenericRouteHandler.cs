using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace Amon.cc.Oidc.SampleApplication.Handler
{
	public class GenericRouteHandler<Handler> : IRouteHandler
	where Handler : IHttpHandler, new()
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new Handler();
		}
	}
}
