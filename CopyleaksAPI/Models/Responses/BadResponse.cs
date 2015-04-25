using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
