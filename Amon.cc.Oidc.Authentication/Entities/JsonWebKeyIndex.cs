using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Amon.cc.Oidc.Authentication.Entities
{
	/// <summary>
	/// JsonWebKeyIndex represents a list of <see cref="JsonWebKey"/>
	/// </summary>
	[DataContract]
	internal class JsonWebKeyIndex
	{
		/// <summary/>
		[DataMember(Name="keys")]
		public List<JsonWebKey> Keys { get; set; }
	}
}
