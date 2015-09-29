using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Copyleaks.SDK.API.Exceptions;
using Copyleaks.SDK.API.Extentions;
using Copyleaks.SDK.API.Models.Requests;
using Copyleaks.SDK.API.Models.Responses;
using Copyleaks.SDK.API.Properties;
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <returns></returns>
		public async Task<ScannerProcess> CreateProcessAsync(string url)
		{
			if (this.Token == null)
				throw new UnauthorizedAccessException("Empty token!");
			else
				this.Token.Validate();

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(HttpContentTypes.Json, this.Token);

				CreateCommandRequest req = new CreateCommandRequest() { URL = url };

				HttpResponseMessage msg;
				if (File.Exists(url))
				{
					FileInfo localFile = new FileInfo(url);
					// Local file. Need to upload it to the server.

					using (var content = new MultipartFormDataContent("Upload----" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)))
					{
						using (FileStream stream = File.OpenRead(url))
						{
							content.Add(new StreamContent(stream, (int)stream.Length), "document", Path.GetFileName(url));
							msg = client.PostAsync(Resources.ServiceVersion + "/detector/create-by-file", content).Result;
						}
					}
				}
				else
				{
					// Internet path. Just submit it to the server.
					HttpContent content = new StringContent(
						JsonConvert.SerializeObject(req),
						Encoding.UTF8,
						HttpContentTypes.Json);
					msg = client.PostAsync(Resources.ServiceVersion + "/detector/create-by-url", content).Result;
				}

				if (!msg.IsSuccessStatusCode)
				{
					var errorJson = await msg.Content.ReadAsStringAsync();
					var errorObj = JsonConvert.DeserializeObject<BadResponse>(errorJson);
					if (errorObj == null)
						throw new CommandFailedException(msg.StatusCode);
					else
						throw new CommandFailedException(errorObj.Message, msg.StatusCode);
				}

				string json = await msg.Content.ReadAsStringAsync();

				CreateResourceResponse response = JsonConvert.DeserializeObject<CreateResourceResponse>(json);
				return new ScannerProcess(this.Token, response.ProcessId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url">The url of the content to scan</param>
		/// <exception cref="UnauthorizedAccessException">When security-token is undefined or expired</exception>
		/// <exception cref="ArgumentOutOfRangeException">When the input url schema is diffrent then HTTP and HTTPS</exception>
		/// <returns></returns>
		public ScannerProcess CreateByUrl(Uri url)
		{
			if (this.Token == null)
				throw new UnauthorizedAccessException("Empty token!");
			else
				this.Token.Validate();

			if (url.Scheme != "http" && url.Scheme != "https")
				throw new ArgumentOutOfRangeException(nameof(url), "Allowed protocols: HTTP, HTTPS");

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(HttpContentTypes.Json, this.Token);

				CreateCommandRequest req = new CreateCommandRequest() { URL = url.AbsoluteUri };

				HttpResponseMessage msg;
				// Internet path. Just submit it to the server.
				HttpContent content = new StringContent(
					JsonConvert.SerializeObject(req),
					Encoding.UTF8,
					HttpContentTypes.Json);
				msg = client.PostAsync(Resources.ServiceVersion + "/detector/create-by-url", content).Result;

				if (!msg.IsSuccessStatusCode)
				{
					var errorJson = msg.Content.ReadAsStringAsync().Result;
					var errorObj = JsonConvert.DeserializeObject<BadResponse>(errorJson);
					if (errorObj == null)
						throw new CommandFailedException(msg.StatusCode);
					else
						throw new CommandFailedException(errorObj.Message, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;

				CreateResourceResponse response = JsonConvert.DeserializeObject<CreateResourceResponse>(json);
				return new ScannerProcess(this.Token, response.ProcessId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="localfile"></param>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <returns></returns>
		public ScannerProcess CreateByFile(FileInfo localfile)
		{
			if (this.Token == null)
				throw new UnauthorizedAccessException("Empty token!");
			else
				this.Token.Validate();

			if (!localfile.Exists)
				throw new FileNotFoundException("File not found!", localfile.FullName);

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(HttpContentTypes.Json, this.Token);

				CreateCommandRequest req = new CreateCommandRequest()
				{
					URL = localfile.FullName
				};

				HttpResponseMessage msg;
				// Local file. Need to upload it to the server.

				using (var content = new MultipartFormDataContent("Upload----" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)))
				using (FileStream stream = localfile.OpenRead())
				{
					content.Add(new StreamContent(stream, (int)stream.Length), "document", Path.GetFileName(localfile.Name));
					msg = client.PostAsync(Resources.ServiceVersion + "/detector/create-by-file", content).Result;
				}

				if (!msg.IsSuccessStatusCode)
				{
					var errorJson = msg.Content.ReadAsStringAsync().Result;
					var errorObj = JsonConvert.DeserializeObject<BadResponse>(errorJson);
					if (errorObj == null)
						throw new CommandFailedException(msg.StatusCode);
					else
						throw new CommandFailedException(errorObj.Message, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;

				CreateResourceResponse response = JsonConvert.DeserializeObject<CreateResourceResponse>(json);
				return new ScannerProcess(this.Token, response.ProcessId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="localfile"></param>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <returns></returns>
		public ScannerProcess CreateByOcr(FileInfo localfile)
		{
			if (this.Token == null)
				throw new UnauthorizedAccessException("Empty token!");
			else
				this.Token.Validate();

			if (!localfile.Exists)
				throw new FileNotFoundException("File not found!", localfile.FullName);

			using (HttpClient client = new HttpClient())
			{
				client.SetCopyleaksClient(HttpContentTypes.Json, this.Token);

				CreateCommandRequest req = new CreateCommandRequest()
				{
					URL = localfile.FullName
				};

				HttpResponseMessage msg;
				// Local file. Need to upload it to the server.

				using (var content = new MultipartFormDataContent("Upload----" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)))
				using (FileStream stream = localfile.OpenRead())
				{
					content.Add(new StreamContent(stream, (int)stream.Length), "document", Path.GetFileName(localfile.Name));
					msg = client.PostAsync(Resources.ServiceVersion + "/detector/create-by-file-ocr", content).Result;
				}

				if (!msg.IsSuccessStatusCode)
				{
					var errorJson = msg.Content.ReadAsStringAsync().Result;
					var errorObj = JsonConvert.DeserializeObject<BadResponse>(errorJson);
					if (errorObj == null)
						throw new CommandFailedException(msg.StatusCode);
					else
						throw new CommandFailedException(errorObj.Message, msg.StatusCode);
				}

				string json = msg.Content.ReadAsStringAsync().Result;

				CreateResourceResponse response = JsonConvert.DeserializeObject<CreateResourceResponse>(json);
				return new ScannerProcess(this.Token, response.ProcessId);
			}
		}
	}
}
