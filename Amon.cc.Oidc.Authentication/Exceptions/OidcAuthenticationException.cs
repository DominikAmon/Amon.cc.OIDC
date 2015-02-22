using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Exceptions
{
	/// <summary>
	/// Base class for all exceptions of this library
	/// </summary>
	public abstract class OidcAuthenticationException : Exception
	{
		/// <summary/>
		public OidcAuthenticationException(string message) : this(message, null)
		{

		}
		/// <summary/>
		public OidcAuthenticationException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}
