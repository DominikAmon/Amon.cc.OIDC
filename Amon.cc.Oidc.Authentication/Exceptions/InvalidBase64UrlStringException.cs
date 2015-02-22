using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Exceptions
{
	/// <summary>
	/// Thrown when the string could not be parsed for Base64
	/// </summary>
	public class InvalidBase64UrlStringException : OidcAuthenticationException
	{
		/// <summary/>
		public InvalidBase64UrlStringException() : this(null)
		{

		}
		/// <summary/>
		public InvalidBase64UrlStringException(Exception innerException)
			: base("Invalid Base64 Url string", innerException)
		{

		}
	}
}
