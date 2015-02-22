using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Exceptions
{
	/// <summary>
	/// Thrown when an error during the authentication process at google occours
	/// </summary>
	public class AuthenticationException : OidcAuthenticationException
	{
		/// <summary/>
		public string State { get; private set; }
		/// <summary/>
		public string ErrorCode { get; private set; }

		/// <summary/>
		public AuthenticationException(string errorCode, string state) : base(String.Format("Authentication failed: {0}", errorCode))
		{
			State = state;
			ErrorCode = errorCode;
		}
	}
}
