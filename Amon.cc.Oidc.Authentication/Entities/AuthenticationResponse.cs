using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// Authentication Response after a successful authentication porcess
	/// </summary>
	public class AuthenticationResponse
	{
		public AuthenticationResponse(string state, IDTokenPayload idTokenPayload, UserInformation userInformation)
		{
			State = state;
			IDTokenPayload = idTokenPayload;
			UserInformation = userInformation;
		}
		/// <summary>
		/// IDTokenPayload parsed from the response
		/// </summary>
		public IDTokenPayload IDTokenPayload { get; set; }
		/// <summary>
		/// The state that has been sent from the initial request that could be used for the return url for example
		/// </summary>
		public string State { get; set; }
		/// <summary>
		/// UserInformation of a user, if requested
		/// </summary>
		public UserInformation UserInformation { get; set; }
	}
}
