using System;
using System.Net.Http;
using System.Net.Http.Headers;
namespace Copyleaks.SDK.API.Extentions
{
	internal static class HttpClientHelper
	{
		internal static void SetCopyleaksClient(this HttpClient client, string contentType)
		{
			client.BaseAddress = UsersAuthentication.SERVICE_URL;
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
		}
		internal static void SetCopyleaksClient(this HttpClient client, string contentType, LoginToken SecurityToken)
		{
			client.SetCopyleaksClient(contentType);
			client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", "Bearer", SecurityToken.Token));
		}
	}
}
