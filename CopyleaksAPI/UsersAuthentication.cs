using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Copyleaks.SDK.API.Properties;
using Newtonsoft.Json;

namespace Copyleaks.SDK.API
{
	public static class UsersAuthentication
	{
		internal static readonly Uri SERVICE_URL = new Uri(Resources.ServiceEntryPoint);

		/// <summary>
		/// Log-in into copyleaks authentication system.
		/// </summary>
		/// <param name="username">User name</param>
		/// <param name="apiKey">Password</param>
		/// <returns>Login Token to use while accessing resources.</returns>
		/// <exception cref="ArgumentException">Occur when the username and\or password is empty</exception>
		/// <exception cref="JsonException">ALON</exception>
		public static LoginToken Login(string username, string apiKey)
		{
			if (string.IsNullOrEmpty(username))
				throw new ArgumentException("Cannot be empty!", "username"); // ALON
			else if (string.IsNullOrEmpty(apiKey))
				throw new ArgumentException("Cannot be empty!", "password"); // ALON

			LoginToken token;
			using (WebClient client = new WebClient())
			{
				client.Headers.Add(HttpRequestHeader.UserAgent, "CopyleaksSDK/1.0");
				client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				string json = client.UploadString(
					SERVICE_URL + "Token",
					string.Format("username={0}&password={1}&grant_type=password", username, apiKey)
				);
				if (string.IsNullOrEmpty(json))
					throw new JsonException("Server return empty string. Probably, bad request sent."); // ALON

				token = JsonConvert.DeserializeObject<LoginToken>(json);
				if (token == null)
					throw new JsonException("Unable to parse server response."); // ALON
			}

			return token;
		}
	}
}
