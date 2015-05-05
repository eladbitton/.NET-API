
namespace Copyleaks.SDK.API.Models.Responses
{
	internal class BadResponse
	{
		public string Message { get; set; }

		public override string ToString()
		{
			return this.Message;
		}
	}
}
