using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// Google+ Profile Information
	/// </summary>
	[DataContract]
	public class UserInformation
	{
		/// <summary>
		/// Kind of Google+ Information Type. Usually this value is plus#personOpenIdConnect
		/// </summary>
		[DataMember(Name="kind")]
		public string Kind { get; set; }

		/// <summary>
		/// male or female
		/// </summary>
		[DataMember(Name = "gender")]
		public string Gender { get; set; }

		/// <summary>
		/// Unique ID of a user
		/// </summary>
		[DataMember(Name = "sub")]
		public string UserUniqueIdentifier { get; set; }

		/// <summary>
		/// The full name
		/// </summary>
		[DataMember(Name = "name")]
		public string FullName { get; set; }

		/// <summary>
		/// first name of a user
		/// </summary>
		[DataMember(Name = "given_name")]
		public string GivenName { get; set; }

		/// <summary>
		/// Surname of a user
		/// </summary>
		[DataMember(Name = "family_name")]
		public string FamilyName { get; set; }

		/// <summary>
		/// Google+ profile address
		/// </summary>
		[DataMember(Name = "profile")]
		public string ProfileUrl { get; set; }

		/// <summary>
		/// Url to profile picture (usually with a size parameter of 50px)
		/// </summary>
		[DataMember(Name = "picture")]
		public string Picture { get; set; }

		/// <summary>
		/// Emailaddress
		/// </summary>
		[DataMember(Name = "email")]
		public string Email { get; set; }

		/// <summary>
		/// Confirmed Emailadress
		/// </summary>
		[DataMember(Name = "email_verified")]
		public string EmailVerified { get; set; }

		/// <summary>
		/// Language of a user
		/// </summary>
		[DataMember(Name = "locale")]
		public string Locale { get; set; }

		/// <summary>
		/// Hosted Domain
		/// </summary>
		[DataMember(Name = "hd")]
		public string HostedDomain { get; set; }
	}
}
