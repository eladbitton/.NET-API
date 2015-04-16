using System;

namespace Copyleaks.SDK.API.Models.Responses
{
	internal class CheckStatusResponse
	{
		public Guid ProcessId { get; set; }

		public string Status { get; set; }
	}
}
