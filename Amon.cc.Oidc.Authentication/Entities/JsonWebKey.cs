using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// A JsonWebKey entity (JWK)
	/// </summary>
	[DataContract]
	internal class JsonWebKey
	{
		/// <summary/>
		[DataMember(Name="e")]
		public string Exponent { get; set; }

		/// <summary/>
		[DataMember(Name = "n")]
		public string Modulus { get; set; }

		/// <summary/>
		[DataMember(Name = "use")]
		public string PublicKeyUse { get; set; }

		/// <summary/>
		[DataMember(Name = "alg")]
		public string Algorithm { get; set; }

		/// <summary/>
		[DataMember(Name = "kid")]
		public string KeyID { get; set; }

		/// <summary/>
		[DataMember(Name = "kty")]
		public string KeyType { get; set; }

	}
}
