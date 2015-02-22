using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Exceptions
{
	/// <summary>
	/// Thrown when the signature could not be verified
	/// </summary>
	public class SignatureVerificationException : OidcAuthenticationException
	{
		/// <summary/>
		public SignatureVerificationException()
			: base("The signature verification failed")
		{
		}
	}
}
