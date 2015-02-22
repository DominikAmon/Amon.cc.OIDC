using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Exceptions
{
	/// <summary>
	/// Thrown when the used hash is not supported
	/// </summary>
	public class HashAlgorithmNotSupportedException : OidcAuthenticationException
	{
		/// <summary/>
		public HashAlgorithmNotSupportedException(string unsupportedHashAlgorithm) : base(String.Format("The hash algorithm {0} is not supported", unsupportedHashAlgorithm))
		{

		}
	}
}
