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
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

using Amon.cc.Oidc.Authentication.Exceptions;

namespace Amon.cc.Oidc.Authentication
{
	/// <summary>
	/// Utility for Deserialization and conversion
	/// </summary>
	internal static class ConversionUtility
	{
		/// <summary>
		/// DeSerializerObject to an object
		/// </summary>
		/// <typeparam name="T">Type to deserialize to</typeparam>
		/// <param name="json">Json Byte-Array</param>
		/// <returns>Instance of type T or null</returns>
		internal static T DeSerializerObject<T>(byte[] json)
		{
			using (MemoryStream jsonStream = new MemoryStream(json))
			{
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
				return (T)serializer.ReadObject(jsonStream);
			}
		}
		/// <summary>
		/// DeSerializerObject to an object as an overload to pass on
		/// a json string
		/// </summary>
		/// <typeparam name="T">Type to deserialize to</typeparam>
		/// <param name="json">Json Strong</param>
		/// <returns>Instance of type T or null</returns>
		internal static T DeSerializerObject<T>(string json)
		{
			return DeSerializerObject<T>(UTF8Encoding.UTF8.GetBytes(json));
		}

		/// <summary>
		/// Decodes a Base64 UrlEncoded value and transforms it to a byte-array
		/// </summary>
		/// <remarks>
		/// A modified version of the method of the Base64UrlDecoding Method found on Stackoverflow:
		/// http://stackoverflow.com/a/10106800/1099519
		/// </remarks>
		/// <param name="encodedValue">Value to be decoded</param>
		/// <returns>decoded value</returns>
		/// <exception cref="InvalidBase64UrlStringException">Thrown, when there was an exception during the decoding process</exception>
		internal static byte[] Base64UrlDecode(string encodedValue)
		{
			try
			{
				encodedValue = encodedValue.Replace("-", "+").Replace("_", "/"); // 62nd & 63rd char of encoding			
				switch (encodedValue.Length % 4) // Pad with trailing '='s
				{
					case 0: break; // No pad chars in this case
					case 2: encodedValue += "=="; break; // Two pad chars
					case 3: encodedValue += "="; break; // One pad char
					default: throw new InvalidBase64UrlStringException();
				}
				return Convert.FromBase64String(encodedValue); // Standard base64 decoder
			}
			catch (InvalidBase64UrlStringException) 
			{
				throw; //re-throw exception
			} 
			catch (Exception ex)
			{
				throw new InvalidBase64UrlStringException(ex);
			}
		}
	}
}
