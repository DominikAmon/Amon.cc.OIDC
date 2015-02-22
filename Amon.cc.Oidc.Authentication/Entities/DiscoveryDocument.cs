using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// Urls of certain google api endpoints.
	/// Based on https://developers.google.com/accounts/docs/OpenIDConnect#discovery 
	/// for all the needed properties
	/// </summary>	
	[DataContract]
	internal class DiscoveryDocument 
	{
		/// <summary/>		
		[DataMember(Name = "authorization_endpoint")]
		public string AuthorizationEndpoint { get; set; }

		/// <summary/>
		[DataMember(Name = "token_endpoint")]
		public string TokenEndpoint { get; set; }

		/// <summary/>
		[DataMember(Name = "jwks_uri")]
		public string JsonWebKeysUri { get; set; }

		/// <summary/>
		[DataMember(Name = "userinfo_endpoint")]
		public string UserinfoEndpoint { get; set; }

	}
}
