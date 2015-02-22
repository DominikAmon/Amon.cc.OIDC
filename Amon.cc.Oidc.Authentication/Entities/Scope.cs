using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Entities
{	
	/// <summary>
	/// Scope of data to enquiry from Google. Can be used as a bit mask (separate values in the request with pipes | ) 
	/// Regarding to the Google's documentation, the scope has to begin with openid and then include profile or email or both
	/// </summary>
	/// <remarks>For details see https://developers.google.com/accounts/docs/OpenIDConnect#scope-param </remarks>
	public enum Scope
	{		
		OpenID,
		Email,
		Profile
	}
}
