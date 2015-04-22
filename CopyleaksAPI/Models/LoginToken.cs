using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Copyleaks.SDK.API
{
	public class LoginToken
	{
		public LoginToken(string token, DateTime issued, DateTime expire)
		{
			this.Token = token;
			this.Issued = issued;
			this.Expire = expire;
		}

		#region Members & Properties

		[JsonProperty(PropertyName = "access_token")]
		public string Token { get; private set; }

		[JsonProperty(PropertyName = ".issued")]
		public DateTime Issued { get; private set; }

		[JsonProperty(PropertyName = ".expires")]
		public DateTime Expire { get; private set; }

		[JsonProperty(PropertyName = "userName")]
		public string UserName { get; private set; }

		[JsonProperty(PropertyName = "token_type")]
		public string TokenType { get; private set; }

		#endregion

		/// <summary>
		/// Vlidate that the token is valid. If isn't valid, throw UnauthorizedAccessException.
		/// </summary>
		/// <exception cref="UnauthorizedAccessException">This token is expired</exception>
		public void Validate()
		{
			if (DateTime.UtcNow > this.Expire)
				throw new UnauthorizedAccessException(string.Format("This token expired on {0}", this.Expire));
		}

		public override string ToString()
		{
			return this.Token;
		}
	}
}
