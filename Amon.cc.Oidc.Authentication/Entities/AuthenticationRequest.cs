using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// For details see https://developers.google.com/accounts/docs/OpenIDConnect#authenticationuriparameters
	/// </summary>
	public class AuthenticationRequest
	{
		/// <summary>
		/// A state that will be sent back after authentication
		/// </summary>
		public string State { get; set; }		
		/// <summary>
		/// Can be used for pre-slection of an account
		/// </summary>
		public string LoginHint { get; set; }
		/// <summary>
		/// Open ID 2.0 Protocol (not OAuth2.0), used for migration purposes
		/// See details: https://developers.google.com/accounts/docs/OpenID
		/// </summary>
		public string OpenIDRealm { get; set; }
		/// <summary>
		/// See details https://developers.google.com/accounts/docs/OpenIDConnect#hd-param
		/// </summary>
		public string HostedDomain { get; set; }
		/// <summary>
		/// Scope values to receive information for (bit mask).
		/// Regarding to the Google's documentation, the scope has to begin with openid and then include profile or email or both
		/// </summary>
		/// <remarks>For details see https://developers.google.com/accounts/docs/OpenIDConnect#scope-param </remarks>
		public Scope Scope { get; set; }
		/// <summary>
		/// Authentication request class to setup a request for the AuthenticationUtility.Authenticate method.
		/// </summary>
		/// <param name="scope">The required scope to receive information for. 
		/// Regarding to the Google's documentation, the scope has to begin with openid and then include profile or email or both.
		/// For details see https://developers.google.com/accounts/docs/OpenIDConnect#scope-param
		/// </param>		
		public AuthenticationRequest(Scope scope)
		{
			Scope = scope;
		}
	}
}
