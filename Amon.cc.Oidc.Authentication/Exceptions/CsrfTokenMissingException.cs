using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Exceptions
{
	/// <summary>
	/// Thrown when CSRF token is missing
	/// </summary>
	public class CsrfTokenMissingException : OidcAuthenticationException
	{
		/// <summary/>
		public CsrfTokenMissingException() : base("CSRF Token is missing")
		{

		}
	}
}
