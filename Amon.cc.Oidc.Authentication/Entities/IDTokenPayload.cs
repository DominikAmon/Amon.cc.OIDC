using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// Entity Basic user information
	/// See details at
	/// https://developers.google.com/accounts/docs/OpenIDConnect#obtainuserinfo
	/// </summary>
	[DataContract]
	public class IDTokenPayload
	{
		/// <summary/>		
		[DataMember(Name = "iss")]
		public string IssuerIdentifier { get; set; }

		/// <summary/>		
		[DataMember(Name = "at_hash")]
		public string AccessTokenHash { get; set; }

		/// <summary/>		
		[DataMember(Name = "email_verified")]
		public bool EmailVerified { get; set; }

		/// <summary/>		
		[DataMember(Name = "sub")]
		public string UserUniqueIdentifier { get; set; }

		/// <summary/>		
		[DataMember(Name = "azp")]
		public string AuthorizedPresenter { get; set; }

		/// <summary/>		
		[DataMember(Name = "email")]
		public string Email { get; set; }

		/// <summary/>		
		[DataMember(Name = "aud")]
		public string Audience { get; set; }

		/// <summary/>		
		[DataMember(Name = "iat")]
		public int IssueTimestampInSeconds { get; set; } //since 1.1.1970

		[DataMember(Name = "exp")]
		public int ExpirationTimestampInSeconds { get; set; } //since 1.1.1970

		/// <summary>
		/// Returning the issed timestamp in UTC
		/// </summary>
		public DateTime IssueTimestamp
		{
			get
			{
				return new DateTime(1970, 1, 1, 0,0,0, DateTimeKind.Utc).AddSeconds(IssueTimestampInSeconds);
			}
		}

		/// <summary>
		/// Returning the expiration timestamp in UTC
		/// </summary>
		public DateTime ExpirationTimestamp
		{
			get
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ExpirationTimestampInSeconds);
			}
		}
	}
}
