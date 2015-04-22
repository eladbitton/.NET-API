using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Copyleaks.SDK.API.Properties;
using Newtonsoft.Json;
using Copyleaks.SDK.API.Extentions;
using Copyleaks.SDK.API.Exceptions;
using Copyleaks.SDK.API.Models.Responses;

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
				throw new ArgumentException("Username is mandatory.", "username");
			else if (string.IsNullOrEmpty(apiKey))
				throw new ArgumentException("Password is mandatory.", "password");

			LoginToken token;
			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.UrlEncoded);
				HttpResponseMessage msg = client.PostAsync("Token", new FormUrlEncodedContent(new[] 
					{
						new KeyValuePair<string, string>("username", username),
						new KeyValuePair<string, string>("password", apiKey),
						new KeyValuePair<string, string>("grant_type", "password")
					})).Result;

				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = msg.Content.ReadAsStringAsync().Result;
					BadLoginResponse response = JsonConvert.DeserializeObject<BadLoginResponse>(errorResponse);
					if (response == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(response.Description, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;

				if (string.IsNullOrEmpty(json))
					throw new JsonException("This request could not be processed.");

				token = JsonConvert.DeserializeObject<LoginToken>(json);
				if (token == null)
					throw new JsonException("Unable to process server response.");
			}

			return token;
		}
	}
}
