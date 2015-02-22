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
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;

using Amon.cc.Oidc.Authentication.Entities;

namespace Amon.cc.Oidc.Authentication
{
	/// <summary>
	/// Configuration settings are handled by this class
	/// </summary>
	internal static class Configuration
	{
		private static DiscoveryDocument _discoveryDocument = null;
		/// <summary>
		/// The document located usally located https://accounts.google.com/.well-known/openid-configuration
		/// and deserialized to an <see cref="DiscoveryDocument"/> entity.
		/// </summary>
		private static DiscoveryDocument DiscoveryDocument
		{
			get
			{
				if (_discoveryDocument == null)
				{
					_discoveryDocument = WebClientUtility.GetDiscoveryDocument();
				}
				return _discoveryDocument;
			}
		}

		/// <summary>
		/// Returns a configured proxy address.
		/// </summary>
		internal static string Proxy
		{
			get
			{
				return ConfigurationManager.AppSettings["googleRequestsProxyAddress"];
			}
		}

		/// <summary>
		/// Returns the configured ClientID (provided by Google)
		/// </summary>
		internal static string ClientID
		{
			get
			{
				return ConfigurationManager.AppSettings["googleClientID"];
			}
		}
		/// <summary>
		/// Returns the configured ClientSecret (provided by Google)
		/// </summary>
		internal static string ClientSecret
		{
			get
			{
				return ConfigurationManager.AppSettings["googleClientSecret"];				
			}
		}
		/// <summary>
		/// Returns the configured RedirectURI (needs to be configrued at Google as well, to identify your application!)
		/// </summary>
		internal static string RedirectURI
		{
			get
			{
				return ConfigurationManager.AppSettings["googleRedirectURI"];
			}
		}

		/// <summary>
		/// Subclass for Endpoint address
		/// </summary>
		internal static class Endpoints
		{
			/// <summary>
			/// Uri of the OpenIDConfiguration / DiscoveryDocument
			/// </summary>
			internal static Uri OpenIDConfigurationURI
			{
				get
				{
					return new Uri(ConfigurationManager.AppSettings["googleOidcDiscoveryDocument"]);
				}
			}
			/// <summary>
			/// Address of the JWKs
			/// </summary>
			internal static string JsonWebKeysUri
			{
				get
				{
					return DiscoveryDocument.JsonWebKeysUri;
				}
			}
			/// <summary>
			/// Authorization Address
			/// </summary>
			internal static string Authorization
			{
				get
				{
					return DiscoveryDocument.AuthorizationEndpoint;
				}
			}
			/// <summary>
			/// Token Address
			/// </summary>
			internal static string Token
			{
				get
				{
					return DiscoveryDocument.TokenEndpoint;
				}
			}

			/// <summary>
			/// Userinfo Endpoint
			/// </summary>
			internal static string UserinfoEndpoint
			{
				get
				{
					return DiscoveryDocument.UserinfoEndpoint;
				}
			}
		}
	}
}
