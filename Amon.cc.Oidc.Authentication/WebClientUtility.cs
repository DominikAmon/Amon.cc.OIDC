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
using System.Net;
using System.Collections.Specialized;

using Amon.cc.Oidc.Authentication.Entities;

namespace Amon.cc.Oidc.Authentication
{
	/// <summary>
	/// This class is responsible for all server communications to Google
	/// </summary>
	internal static class WebClientUtility
	{
		/// <summary>
		/// Proxy server used to connect to Google
		/// </summary>
		internal static IWebProxy Proxy
		{
			get
			{
				if (String.IsNullOrWhiteSpace(Configuration.Proxy))
				{
					return WebRequest.GetSystemWebProxy();
				}
				return new WebProxy(Configuration.Proxy);
			}
		}

		/// <summary>
		/// Receive ID Token from Google
		/// </summary>
		/// <param name="parameters">List of necessary parameters</param>
		/// <remarks>https://developers.google.com/accounts/docs/OpenIDConnect#exchangecode</remarks>
		/// <returns></returns>
		internal static TokenInformation GetIDTokenPayload(NameValueCollection parameters)
		{
			using (WebClient client = new WebClient() { Proxy = Proxy })
			{
				return ConversionUtility.DeSerializerObject<TokenInformation>(client.UploadValues(Configuration.Endpoints.Token, "POST", parameters));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal static JsonWebKeyIndex GetJsonWebKeyIndex()
		{
			using (WebClient client = new WebClient() { Proxy = Proxy })
			{
				return ConversionUtility.DeSerializerObject<JsonWebKeyIndex>(client.DownloadData(Configuration.Endpoints.JsonWebKeysUri));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <remarks>https://developers.google.com/accounts/docs/OpenIDConnect#discovery</remarks>
		internal static DiscoveryDocument GetDiscoveryDocument()
		{
			using (WebClient client = new WebClient() { Proxy = Proxy })
			{
				return ConversionUtility.DeSerializerObject<DiscoveryDocument>(client.DownloadData(Configuration.Endpoints.OpenIDConfigurationURI));
			}
		}

		/// <summary>
		/// Receives profile information for a user
		/// </summary>
		/// <param name="accessToken">Access token to receive information</param>
		/// <returns>UserInformation</returns>
		/// <remarks>
		/// In order to use this method, the scope "profile" AND "email" must be set AND Google+ API needs to turned on.
		/// Note: the limit of calls per day is 10000 requests.
		/// If 10000 request/day is exceeded or Google+ API is not turned on, this method call will result in an error 403.
		/// You may want to do some additional exception handling for that cases here!
		/// </remarks>
		internal static UserInformation GetUserInformation(string accessToken)
		{
			using (WebClient client = new WebClient() { Proxy = Proxy })
			{
				string url = String.Concat(Configuration.Endpoints.UserinfoEndpoint, "?access_token=", accessToken);
				return ConversionUtility.DeSerializerObject<UserInformation>(client.DownloadData(url));
			}
		}
	}
}
