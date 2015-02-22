using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// ID Token data
	/// See https://developers.google.com/accounts/docs/OpenIDConnect#exchangecode
	/// </summary>
	[DataContract]
	internal class TokenInformation
	{
		/// <summary/>
		[DataMember(Name="access_token")]			
		public string AccessToken { get; set; }
		
		/// <summary/>
		[DataMember(Name="token_type")]
		public string TokenType { get; set; }

		/// <summary/>
		[DataMember(Name="expires_in")]
		public int ExpiresIn { get; set; }

		/// <summary/>
		[DataMember(Name="id_token")]
		public string IDToken { get; set; }		
		
	}
}
