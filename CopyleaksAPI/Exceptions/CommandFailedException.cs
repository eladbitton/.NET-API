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

		internal CommandFailedException(string message, HttpStatusCode code) : 
			base(string.Format("Execution failed with the code {0}:\n{1}", code, message))
		{
			this.ErrorCode = code;
		}
	}
}
