using System;
using System.IO;
using System.Threading;
using Copyleaks.SDK.API;
using Copyleaks.SDK.API.Models;

namespace Copyleaks.SDK.SampleCode
{
	public class Scanner
	{
		#region Members & Properties

		const int ISCOMPLETED_SLEEP = 5000;

		public string Username { get; set; }

		public string ApiKey { get; set; }

		protected LoginToken Token { get; set; }

		public uint Credits
		{
			get
			{
				if (this.Token == null)
					throw new UnauthorizedAccessException();
				else
					this.Token.Validate();

				return UsersAuthentication.CountCredits(this.Token);
			}

		}


		#endregion

		public Scanner(string username, string APIKey)
		{
			this.Token = UsersAuthentication.Login(username, APIKey); // This security token can be use multiple times, until it will be expired (48 hours).
		}

		public ResultRecord[] ScanUrl(Uri url, Uri httpCallback = null)
		{
			// Create a new process on server.
			Detector detector = new Detector(this.Token);
			ScannerProcess process = detector.CreateByUrl(url, httpCallback);

			// Waiting to process to be finished.
			while (!process.IsCompleted())
				Thread.Sleep(ISCOMPLETED_SLEEP);

			// Getting results.
			return process.GetResults();
		}

		public ResultRecord[] ScanLocalTextualFile(FileInfo file, Uri httpCallback = null)
		{
			// Create a new process on server.
			Detector detector = new Detector(this.Token);
			ScannerProcess process = detector.CreateByFile(file, httpCallback);

			// Waiting to process to be finished.
			while (!process.IsCompleted())
				Thread.Sleep(ISCOMPLETED_SLEEP);

			// Getting results.
			return process.GetResults();
		}
	}
}
