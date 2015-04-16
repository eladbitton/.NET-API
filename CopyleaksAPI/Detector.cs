using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Copyleaks.SDK.API.Exceptions;
using Copyleaks.SDK.API.Extentions;
using Copyleaks.SDK.API.Models;
using Copyleaks.SDK.API.Models.Requests;
using Copyleaks.SDK.API.Models.Responses;
using Newtonsoft.Json;

namespace Copyleaks.SDK.API
{
	public class Detector
	{
		private LoginToken Token { get; set; }

		public Detector(LoginToken token)
		{
			this.Token = token;
		}

		public async Task<ScannerProcess> CreateProcessAsync(string url)
		{
			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(ContentType.Json, this.Token);

				CreateCommandRequest req = new CreateCommandRequest() { URL = url };

				HttpContent content = new StringContent(
					JsonConvert.SerializeObject(req),
					Encoding.UTF8,
					ContentType.Json);

				HttpResponseMessage msg = await client.PostAsync("detector/create", content);

				

				//string json = client.UploadString(string.Format("{0}detector/create", UsersAuthentication.SERVICE_URL),
				//		JsonConvert.SerializeObject(req));

				if (!msg.IsSuccessStatusCode)
					throw new CommandFailedException(msg.ReasonPhrase, msg.StatusCode);

				string json = await msg.Content.ReadAsStringAsync();
				//return JsonConvert.DeserializeObject<ResultRecord[]>(json);

				CreateResourceResponse response = JsonConvert.DeserializeObject<CreateResourceResponse>(json);
				return new ScannerProcess(this.Token, response.ProcessId);
			}
		}

		//public bool IsCompleted(Guid processId)
		//{
		//	using (WebClient client = new WebClient())
		//	{
		//		client.Headers[HttpRequestHeader.ContentType] = "application/json";
		//		client.Headers.Add("Authorization", string.Format("{0} {1}", "Bearer", this.Token.Token));

		//		// TODO : delete
		//		//CheckStatusRequest request = new CheckStatusRequest() { CommandId = commandId };


		//		string json = client.DownloadString(string.Format("{0}Detector/{1}/Status", UsersAuthentication.SERVICE_URL, processId));

		//		CheckStatusResponse response = JsonConvert.DeserializeObject<CheckStatusResponse>(json);
		//		return response.Status == "Finished";
		//	}
		//}

		//public ResultRecord[] GetResults(Guid processId)
		//{
		//	using (WebClient client = new WebClient())
		//	{
		//		client.Headers[HttpRequestHeader.ContentType] = "application/json";
		//		client.Headers.Add("Authorization", string.Format("{0} {1}", "Bearer", this.Token.Token));

		//		string json = client.DownloadString(string.Format("{0}Detector/{1}/Result", UsersAuthentication.SERVICE_URL, processId));

		//		return JsonConvert.DeserializeObject<ResultRecord[]>(json);
		//	}
		//}

		//public string Work()
		//{
		//	using (AdvancedWebClient client = new AdvancedWebClient())
		//	{
		//		client.Headers[HttpRequestHeader.ContentType] = "application/json";
		//		client.Headers.Add("Authorization", string.Format("{0} {1}", "Bearer", this.Token.Token));
		//		return client.UploadString(UsersAuthentication.SERVICE_URL + "Detector", "");
		//	}
		//}

		//public async Task<ResultRecord[]> DetectCopiesAsync(string url)
		//{
		//	using (WebClient client = new WebClient())
		//	{
		//		client.Headers[HttpRequestHeader.ContentType] = "application/json";
		//		client.Headers.Add("Authorization",
		//			string.Format("{0} {1}", "Bearer", this.Token.Token));
		//		string json = await client.DownloadStringTaskAsync(string.Format("{0}Detector/Detect?URL={1}",
		//				UsersAuthentication.SERVICE_URL,
		//				Uri.EscapeUriString(url)));

		//		return JsonConvert.DeserializeObject<ResultRecord[]>(json);
		//	}
		//}
	}
}
