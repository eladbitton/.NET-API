using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Copyleaks.SDK.API.Exceptions;
using Copyleaks.SDK.API.Models;
using Copyleaks.SDK.API.Models.Responses;
using Copyleaks.SDK.API.Extentions;
using Newtonsoft.Json;

namespace Copyleaks.SDK.API
{
	public class ScannerProcess
	{
		#region Members & Properties

		public Guid PID { get; set; }

		private LoginToken SecurityToken { get; set; }

		#endregion

		public ScannerProcess(LoginToken authorizationToken, Guid id)
		{
			this.PID = id;
			this.SecurityToken = authorizationToken;
		}
		#region IsCompleted
		/// <summary>
		/// Checks if the operation is completed.
		/// </summary>
		/// <returns>Return True in case that the operation on is finished by the server</returns>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public async Task<bool> IsCompletedAsync()
		{
			this.SecurityToken.Validate(); // may throw an UnauthorizedAccessException.

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.SecurityToken);

				HttpResponseMessage msg = await client.GetAsync(string.Format("detector/{0}/status", this.PID));
				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = await msg.Content.ReadAsStringAsync();
					BadResponse error = JsonConvert.DeserializeObject<BadResponse>(errorResponse);
					if (error == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(error.Message, msg.StatusCode);
				}

				string json = await msg.Content.ReadAsStringAsync();

				CheckStatusResponse response = JsonConvert.DeserializeObject<CheckStatusResponse>(json);
				return response.Status == "Finished";
			}
		}

		/// <summary>
		/// Checks if the operation is completed.
		/// </summary>
		/// <returns>Return True in case that the operation on is finished by the server</returns>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public bool IsCompleted()
		{
			this.SecurityToken.Validate(); // may throw an UnauthorizedAccessException.

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.SecurityToken);

				HttpResponseMessage msg = client.GetAsync(string.Format("detector/{0}/status", this.PID)).Result;
				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = msg.Content.ReadAsStringAsync().Result;
					BadResponse error = JsonConvert.DeserializeObject<BadResponse>(errorResponse);
					if (error == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(error.Message, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;

				CheckStatusResponse response = JsonConvert.DeserializeObject<CheckStatusResponse>(json);
				return response.Status == "Finished";
			}
		}
		#endregion

		#region GetResults
		/// <summary>
		/// Get the scanning resutls from server.
		/// </summary>
		/// <returns>Scanning results</returns>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public async Task<ResultRecord[]> GetResultsAsync()
		{
			this.SecurityToken.Validate(); // may throw an UnauthorizedAccessException.

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.SecurityToken);

				HttpResponseMessage msg = await client.GetAsync(string.Format("detector/{0}/result", this.PID));
				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = msg.Content.ReadAsStringAsync().Result;
					BadResponse error = JsonConvert.DeserializeObject<BadResponse>(errorResponse);
					if (error == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(error.Message, msg.StatusCode);
				}

				string json = await msg.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<ResultRecord[]>(json);
			}
		}

		/// <summary>
		/// Get the scanning resutls from server.
		/// </summary>
		/// <returns>Scanning results</returns>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public ResultRecord[] GetResults()
		{
			this.SecurityToken.Validate(); // may throw an UnauthorizedAccessException.

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.SecurityToken);

				HttpResponseMessage msg = client.GetAsync(string.Format("detector/{0}/result", this.PID)).Result;
				if (!msg.IsSuccessStatusCode)
				{
					string errorResponse = msg.Content.ReadAsStringAsync().Result;
					BadResponse error = JsonConvert.DeserializeObject<BadResponse>(errorResponse);
					if (error == null)
						throw new JsonException("Unable to process server response.");
					else
						throw new CommandFailedException(error.Message, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;
				return JsonConvert.DeserializeObject<ResultRecord[]>(json);
			}
		}
		#endregion

		#region Delete
		/// <summary>
		/// Delete finished process
		/// </summary>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public async void DeleteAsync()
		{
			this.SecurityToken.Validate(); // may throw an UnauthorizedAccessException.

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.SecurityToken);

				HttpResponseMessage msg = await client.DeleteAsync(string.Format("detector/{0}/delete", this.PID));
				if (!msg.IsSuccessStatusCode)
					throw new CommandFailedException(msg.ReasonPhrase, msg.StatusCode);
			}
		}

		/// <summary>
		/// Delete finished process
		/// </summary>
		/// <exception cref="UnauthorizedAccessException"></exception>
		public void Delete()
		{
			this.SecurityToken.Validate(); // may throw an UnauthorizedAccessException.

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.SecurityToken);

				HttpResponseMessage msg = client.DeleteAsync(string.Format("detector/{0}/delete", this.PID)).Result;
				if (!msg.IsSuccessStatusCode)
					throw new CommandFailedException(msg.ReasonPhrase, msg.StatusCode);
			}
		}
		#endregion
	}
}
