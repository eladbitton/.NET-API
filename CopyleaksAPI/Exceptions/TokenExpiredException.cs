using System;

namespace Copyleaks.SDK.API.Exceptions
{
	public class TokenExpiredException : AccessViolationException
	{
		public TokenExpiredException(LoginToken token) :
			base(string.Format("This token expired on '{0}'!", token.Expire)) 
		{
			this.Token = token;
		}

		public LoginToken Token { get; private set; }
	}
}
