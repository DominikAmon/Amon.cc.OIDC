using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Exceptions
{
	/// <summary>
	/// Thrown when the state is invalid or corrupt
	/// </summary>
	public class InvalidStateException : OidcAuthenticationException
	{
		/// <summary/>
		public string State { get; private set; }
		/// <summary/>
		public InvalidStateException(string state) : base("The state is invalid or potential malicious response")
		{
			State = state;
		}
	}
}
