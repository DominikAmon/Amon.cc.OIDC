using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amon.cc.Oidc.Authentication;
using Amon.cc.Oidc.Authentication.Entities;

namespace Amon.cc.Oidc.SampleApplication
{
	public partial class LoginRedirect : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AuthenticationRequest authenticationRequest = new AuthenticationRequest(Scope.OpenID | Scope.Email | Scope.Profile);
			authenticationRequest.State = Request.Params["ReturnUrl"];	
			AuthenticationUtility.Authenticate(authenticationRequest);
		}
	}
}