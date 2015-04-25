using Newtonsoft.Json;

namespace Copyleaks.SDK.API.Models.Responses
{
	internal class BadLoginResponse
	{
		[JsonProperty(PropertyName = "error")]
		public string Error { get; set; }

		[JsonProperty(PropertyName = "error_description")]
		public string Description { get; set; }
	}
}
