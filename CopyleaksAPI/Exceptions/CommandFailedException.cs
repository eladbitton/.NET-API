using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Copyleaks.SDK.API.Exceptions
{
	public class CommandFailedException : ApplicationException
	{
		public HttpStatusCode ErrorCode { get; private set; }

		public string Description { get; set; }

		internal CommandFailedException(HttpStatusCode code) :
			base(string.Format("HTTP Error code: {0}({1})", code, (int)code))
		{
			this.ErrorCode = code;
		}

		internal CommandFailedException(string message, HttpStatusCode code) : 
			base(string.Format("Execution failed with the code {0}({1}):\n{2}", code, (int)code, message))
		{
			this.ErrorCode = code;
			this.Description = message;
		}
	}
}
