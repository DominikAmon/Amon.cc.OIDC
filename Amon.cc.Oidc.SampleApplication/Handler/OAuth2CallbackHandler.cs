using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Security;
using Amon.cc.Oidc.Authentication.Entities;
using Amon.cc.Oidc.Authentication.Exceptions;
using Amon.cc.Oidc.Authentication;
using System.Web.Security;

namespace Amon.cc.Oidc.SampleApplication.Handler
{
	public class OAuth2CallbackHandler : IHttpHandler, IRequiresSessionState
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{							
			AuthenticationResponse response = AuthenticationUtility.HandleResponse();

			if (response.IDTokenPayload.IssueTimestamp.ToLocalTime() > DateTime.Now && response.IDTokenPayload.ExpirationTimestamp.ToLocalTime() > DateTime.Now)
			{
				//The token is expired or not valid anymore, go back to Root of application now
				context.Response.Redirect("~/", true);				
			}

			/* 
				Use the E-Mail address of the user as an identifier for demonstration purposes
				Note: you may want to use response.IDTokenPayload.UserUniqueIdentifier instead
				that represents the sub parameter as google recommends to do so. See
				the "sub" parameter description at "An ID token's payload":
				https://developers.google.com/accounts/docs/OpenIDConnect#obtainuserinfo
				Before you call SetAuthCookie method, you could also do some additional authorization 
				checks with the given user information.
			*/
			FormsAuthentication.SetAuthCookie(response.IDTokenPayload.Email, false);

			context.Session["UserInformation"] = response.UserInformation;

			//We've but the redirect url in the state before authenticating, so let's go back there now
			context.Response.Redirect(response.State, true);			
			
		}
	}
}
