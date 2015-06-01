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
using System.Threading.Tasks;
using Copyleaks.SDK.API.Models.Responses.Copyleaks.SDK.API.Models.Responses;

namespace Copyleaks.SDK.API
{
	public static class UsersAuthentication
	{
		internal static readonly Uri SERVICE_URL = new Uri(Resources.ServiceEntryPoint);
		private static readonly string LOGIN_PAGE = string.Format("{0}/account/login", Resources.ServiceVersion);

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
#if !DEBUG // For testing purposes
			if (string.IsNullOrEmpty(username))
				throw new ArgumentException("Username is mandatory.", "username");
			else if (string.IsNullOrEmpty(apiKey))
				throw new ArgumentException("Password is mandatory.", "password");
#endif

			LoginToken token;
			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.UrlEncoded);
				HttpResponseMessage msg = client.PostAsync(LOGIN_PAGE, new FormUrlEncodedContent(new[] 
					{
						new KeyValuePair<string, string>("username", username),
						new KeyValuePair<string, string>("apikey", apiKey)
					})).Result;

				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = msg.Content.ReadAsStringAsync().Result;
					BadLoginResponse response = JsonConvert.DeserializeObject<BadLoginResponse>(errorResponse);
					if (response == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(response.Message, msg.StatusCode);
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


		public static async Task<LoginToken> LoginAsync(string username, string apiKey)
		{
			if (string.IsNullOrEmpty(username))
				throw new ArgumentException("Username is mandatory.", "username");
			else if (string.IsNullOrEmpty(apiKey))
				throw new ArgumentException("Password is mandatory.", "apiKey");

			LoginToken token;
			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.UrlEncoded);
				HttpResponseMessage msg = await client.PostAsync(LOGIN_PAGE, new FormUrlEncodedContent(new[] 
					{
						new KeyValuePair<string, string>("username", username),
						new KeyValuePair<string, string>("apikey", apiKey)
					}));

				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = await msg.Content.ReadAsStringAsync();
					BadLoginResponse response = JsonConvert.DeserializeObject<BadLoginResponse>(errorResponse);
					if (response == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(response.Message, msg.StatusCode);
				}

				string json = await msg.Content.ReadAsStringAsync();

				if (string.IsNullOrEmpty(json))
					throw new JsonException("This request could not be processed.");

				token = JsonConvert.DeserializeObject<LoginToken>(json);
				if (token == null)
					throw new JsonException("Unable to process server response.");
			}

			return token;
		}

		public static int CountCredits(LoginToken token)
		{
			token.Validate();

			if (token == null)
				throw new ArgumentException("Username is mandatory.", "username");

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.UrlEncoded);
				client.SetCopyleaksClient(ContentType.Json, token);

				HttpResponseMessage msg = client.GetAsync(string.Format("{0}/account/count-credits", Resources.ServiceVersion)).Result;
				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = msg.Content.ReadAsStringAsync().Result;
					BadLoginResponse response = JsonConvert.DeserializeObject<BadLoginResponse>(errorResponse);
					if (response == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(response.Message, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;

				if (string.IsNullOrEmpty(json))
					throw new JsonException("This request could not be processed.");

				CountCreditsResponse res = JsonConvert.DeserializeObject<CountCreditsResponse>(json);
				if (token == null)
					throw new JsonException("Unable to process server response.");

				return res.Amount;
			}
		}

		public static async Task<int> CountCreditsAsync(LoginToken token)
		{
			token.Validate();

			if (token == null)
				throw new ArgumentNullException("token");

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, token);

				HttpResponseMessage msg = await client.GetAsync(string.Format("{0}/account/count-credits", Resources.ServiceVersion));
				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = await msg.Content.ReadAsStringAsync();
					BadLoginResponse response = JsonConvert.DeserializeObject<BadLoginResponse>(errorResponse);
					if (response == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(response.Message, msg.StatusCode);
				}

				string json = await msg.Content.ReadAsStringAsync();

				if (string.IsNullOrEmpty(json))
					throw new JsonException("This request could not be processed.");

				CountCreditsResponse res = JsonConvert.DeserializeObject<CountCreditsResponse>(json);
				if (token == null)
					throw new JsonException("Unable to process server response.");

				return res.Amount;
			}
		}
	}
}
