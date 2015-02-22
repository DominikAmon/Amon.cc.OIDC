/*
 * Creator: Dominik Amon
 * Last Update: 2015-02-20   
 * Blog: http://www.dominikamon.com/
 * Website: http://www.amon.cc/ 
 * Github: https://github.com/DominikAmon
 * License: https://creativecommons.org/licenses/by/3.0/at/deed.en
 *  
 * Related Blog article for this project: 
 * http://www.dominikamon.com/articles/3091/oidc-lightweight-library-for-aspnet.html
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Threading;

using Amon.cc.Oidc.Authentication.Entities;
using Amon.cc.Oidc.Authentication.Exceptions;

namespace Amon.cc.Oidc.Authentication
{
	/// <summary>
	/// The Amon.cc OIDC AuthenticationUtility is a lightweight implementation
	/// using OIDC (OpenID Connect) with Google that doesn't require any further
	/// additional libraries other than .net4 framework.	
	/// </summary>	
	/// <remarks>
	/// Based on the documentation and whitepapers:
	/// * https://developers.google.com/accounts/cookbook/technologies/OpenID-Connect	
	/// * https://developers.google.com/accounts/docs/OpenIDConnect
	/// 
	/// The method DecodeAndVerifyIDTokenPayload partially uses code found at
	/// https://github.com/NuGet/OpsDashboard/blob/master/NuGetGallery.Dashboard/Infrastructure/JWT.cs#L21 
	/// by Andrew Nurse (https://github.com/anurse)
	/// and https://github.com/johnsheehan/jwt by John Sheehan (https://github.com/johnsheehan/)
	/// </remarks>
	public class AuthenticationUtility
	{
		private const string HashAlgorithm = "SHA256";
		private const string JwtHashAlgorithm = "RS256"; //=RSA SHA256
		private const string CsrfStateCookieKey = "CsrfStateCookie";
		private const string CsrfTokenParameter = "csrfToken";
		private const string ResponseStateParameter = "responseState";
		private const string ReceiveProfileInformation = "profileInfo";

		/// <summary>
		/// Performs the initial authentication request and redirects to the authentication page
		/// </summary>
		/// <remarks>This method redirects directly to the authentication website of Google and because of this,
		/// the current thread will be aborted, which results in a ThreadAbortException.</remarks>		
		public static void Authenticate()
		{
			Authenticate(new AuthenticationRequest(Scope.OpenID | Scope.Email));
		}
		/// <summary>
		/// Performs the initial authentication request and redirects to the authentication page.		
		/// </summary>
		/// <remarks>
		/// For security reasons a CSRF Token is saved in a cookie and sent as a state. The 
		/// CSRF Token will be verified after the response back from Google. 
		/// Details about CSRF and prevention see: 
		/// http://en.wikipedia.org/wiki/Cross-site_request_forgery#Prevention
		/// 
		/// This method redirects directly to the authentication website of Google and because of this,
		/// the current thread will be aborted, which results in a ThreadAbortException.</remarks>
		/// <param name="request">Parametrs for the authentication request</param>
		/// <exception cref="ThreadAbortException">Thrown because of redirect</exception>
		public static void Authenticate(AuthenticationRequest request)
		{
			//Fixed Parameters
			int csrfTokenExpirationInMinutes = 5; //The time the uses has to authenticate before the CSRF Token gets invalid. You may want to increase this value.
			bool csrfTokenHttpsOnlyCookie = false; //Running an https environment? SET THIS PARAMETER TO true! Otherwise stay with false
			string responseType = "code";

			//Parameters by Request
			string loginHint = request.LoginHint; 
			string openIDRealm = request.OpenIDRealm;
			string state = request.State;
			string hd = request.HostedDomain;

			//Parameters by Configuration
			string clientID = Configuration.ClientID;			
			string redirectUri = Configuration.RedirectURI;
			//Setup Scope Parameter
			string scope = String.Join("%20", Enum.GetValues(typeof(Scope)).Cast<Enum>()
							.Where(item => request.Scope.HasFlag(item))
							.ToList()
							.ConvertAll<string>(item => item.ToString().ToLower())
							.ToArray());			

			//Calculated Parameters 
			string csrfToken = SetCsrfToken(csrfTokenExpirationInMinutes, csrfTokenHttpsOnlyCookie);
			string stateParameter = HttpUtility.UrlEncode(String.Concat(ReceiveProfileInformation, "=", (request.Scope.HasFlag(Scope.Profile)).ToString(), "&", CsrfTokenParameter, "=", csrfToken, "&", ResponseStateParameter, "=", HttpUtility.UrlEncode(state)));
			
			//Redirect to Authentication Website at Google with parameters
			HttpContext.Current.Response.Redirect(String.Format("{0}?client_id={1}&response_type={2}&scope={3}&redirect_uri={4}&state={5}&login_hint={6}&openid.realm={7}&hd={8}",
				Configuration.Endpoints.Authorization, clientID, responseType, scope, redirectUri, stateParameter, loginHint, openIDRealm, hd), true);			
		}

		/// <summary>
		/// This method handles the response from Google and returns an <see cref="AuthenticationResponse"/> object
		/// </summary>
		/// <returns>AuthenticationResponse with details such as the IDTokenPayload</returns>
		/// <exception cref="InvalidStateException">Thrown when the CSRF token is missing or there is a mismatch</exception>
		/// <exception cref="AuthenticationException">Thrown when there was an error in the process of authentication at Google</exception>
		/// <exception cref="HashAlgorithmNotSupportedException">Thrown when the used algorithm for hashing is not supported</exception>
		/// <exception cref="SignatureVerificationException">Thrown when the signature seems to be broken</exception>
		public static AuthenticationResponse HandleResponse()
		{
			string code = HttpContext.Current.Request.Params["code"];
			string state = HttpContext.Current.Request.Params["state"];
			string error = HttpContext.Current.Request.Params["error"];
			
			NameValueCollection stateParameters = HttpUtility.ParseQueryString(state);
			string responseState = stateParameters[ResponseStateParameter];
			bool receiveProfileInformation = Boolean.Parse(stateParameters[ReceiveProfileInformation]); 

			if (!AuthenticationUtility.VerifyCsrfTokenInState(stateParameters)) { throw new InvalidStateException(responseState); }
			if (!String.IsNullOrWhiteSpace(error)) { throw new AuthenticationException(error, responseState); }			
			
			NameValueCollection parameters = new NameValueCollection();
			parameters.Add("code", code);
			parameters.Add("client_id", Configuration.ClientID);
			parameters.Add("client_secret", Configuration.ClientSecret);
			parameters.Add("redirect_uri", Configuration.RedirectURI);
			parameters.Add("grant_type", "authorization_code");

			TokenInformation token = WebClientUtility.GetIDTokenPayload(parameters);
			IDTokenPayload idTokenPayload = DecodeAndVerifyIDTokenPayload(token.IDToken, GetRSACryptoServiceProvider(), true);
			UserInformation userInformation = (receiveProfileInformation)?WebClientUtility.GetUserInformation(token.AccessToken):null;
			
			return new AuthenticationResponse(responseState, idTokenPayload, userInformation);
		}

	
		/// <summary>
		/// Converts the JWKs to a list of <see cref="RSACryptoServiceProvider"/>.
		/// The JWKs are being downloaded directly from the link provided in 
		/// the OIDC Discovery Document.
		/// </summary>
		/// <returns>List of RSACryptoServiceProviders</returns>
		private static List<RSACryptoServiceProvider> GetRSACryptoServiceProvider()
		{
			JsonWebKeyIndex jsonWebKeyIndex = WebClientUtility.GetJsonWebKeyIndex();
			List<RSACryptoServiceProvider> providers = new List<RSACryptoServiceProvider>();				
			jsonWebKeyIndex.Keys.ForEach(jsonWebToken =>
			{
				RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
				provider.ImportParameters(new RSAParameters 
				{ 
					Exponent = ConversionUtility.Base64UrlDecode(jsonWebToken.Exponent), 
					Modulus = ConversionUtility.Base64UrlDecode(jsonWebToken.Modulus)
				});
				providers.Add(provider);
			});

			return providers;
		}

		/// <summary>
		/// Creates a CSRF Token and stores it as a cookie.
		/// the created value will be returned
		/// </summary>
		/// <param name="expirationTimeInMinutes">Time the csrf token is valid</param>
		/// <param name="httpsOnly">Defines if the cookie should only be transported over a secure channel</param>
		/// <remarks>You may want to add your own randomizer in here.</remarks>
		/// <returns>The created token</returns>
		private static string SetCsrfToken(int expirationTimeInMinutes, bool httpsOnly)
		{			
			Guid randomID = Guid.NewGuid();
			string csrfToken = randomID.ToString();
			HttpCookie csrfCookie = new HttpCookie(CsrfStateCookieKey, csrfToken);
			//Expire within a short period of time.
			csrfCookie.Expires = DateTime.Now.AddMinutes(expirationTimeInMinutes);  
			csrfCookie.HttpOnly = true; //No access for JavaScript!
			csrfCookie.Secure = httpsOnly; 
			HttpContext.Current.Response.Cookies.Add(csrfCookie);
			return csrfToken;
		}

		/// <summary>
		/// Verifiy if the CSRF Token is valid
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		/// <remarks>Made for security reasons as suggested by Google
		/// https://developers.google.com/accounts/docs/OpenIDConnect#confirmxsrftoken
		/// </remarks>
		private static bool VerifyCsrfTokenInState(NameValueCollection state)
		{
			if (HttpContext.Current.Request.Cookies[CsrfStateCookieKey] == null) { throw new CsrfTokenMissingException(); }

			string csrfToken = state[CsrfTokenParameter];
			string csrfSessionToken = HttpContext.Current.Request.Cookies[CsrfStateCookieKey].Value;

			//Delete CSRF Cookie
			HttpContext.Current.Response.SetCookie(new HttpCookie(CsrfStateCookieKey, String.Empty) { Expires = DateTime.Now.AddDays(-1) });

			//Compare CSRF Cookie with csrf token that has been sent back
			if (String.IsNullOrWhiteSpace(csrfToken) || String.IsNullOrWhiteSpace(csrfSessionToken)) { throw new CsrfTokenMissingException(); }			
			return csrfSessionToken.Equals(csrfToken, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Verifies the signature of the the value with the given RSA Providers from the JSonWebKey
		/// </summary>
		/// <param name="signedData">The data that has been signed</param>
		/// <param name="signature">The signature of the signed data</param>
		/// <param name="rsaCryptoServiceProviders">A list of rsaCryptoServiceProviders where at least one key should match</param>
		/// <returns>true if the signedData is not broken</returns>
		private static bool VerifyRSAHash(byte[] signedData, byte[] signature, List<RSACryptoServiceProvider> rsaCryptoServiceProviders)
		{
			foreach (RSACryptoServiceProvider provider in rsaCryptoServiceProviders)
			{
				if (provider.VerifyData(signedData, HashAlgorithm, signature)) { return true; }
			}
			return false; //None of the keys matched
		}

		/// <summary>
		/// Given a JWT, decode it and return the JSON payload.
		/// </summary>
		/// <remarks>Based on https://github.com/johnsheehan/jwt and https://github.com/NuGet/OpsDashboard/blob/master/NuGetGallery.Dashboard/Infrastructure/JWT.cs </remarks>
		/// <param name="jsonWebToken">The JWT.</param>
		/// <param name="key">The key that was used to sign the JWT.</param>
		/// <param name="verify">Whether to verify the signature (default is true).</param>
		/// <returns>A string containing the JSON payload.</returns>
		/// <exception cref="SignatureVerificationException">Thrown if the verify parameter was true and the signature was NOT valid or if the JWT was signed with an unsupported algorithm.</exception>
		private static IDTokenPayload DecodeAndVerifyIDTokenPayload(string jsonWebToken, List<RSACryptoServiceProvider> rsaCryptoProvider, bool verify)
		{
			JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
			string[] parts = jsonWebToken.Split('.');
			string header = parts[0];
			string payload = parts[1];
			byte[] crypto = ConversionUtility.Base64UrlDecode(parts[2]);

			string headerJson = Encoding.UTF8.GetString(ConversionUtility.Base64UrlDecode(header));
			Dictionary<string, object> headerData = jsonSerializer.Deserialize<Dictionary<string, object>>(headerJson);
			string algorithm = headerData["alg"] as string;
			string payloadJson = Encoding.UTF8.GetString(ConversionUtility.Base64UrlDecode(payload));
			byte[] bytesToSign = Encoding.UTF8.GetBytes(String.Concat(header, ".", payload));

			if (!JwtHashAlgorithm.Equals(algorithm)) { throw new HashAlgorithmNotSupportedException(algorithm); }
			if (verify && !VerifyRSAHash(bytesToSign, crypto, rsaCryptoProvider)) { throw new SignatureVerificationException(); }

			return ConversionUtility.DeSerializerObject<IDTokenPayload>(payloadJson);
		}
	}
}
