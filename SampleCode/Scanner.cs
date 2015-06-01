using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Copyleaks.SDK.API;
using Copyleaks.SDK.API.Models;

namespace Copyleaks.SDK.SampleCode
{
	public class Scanner
	{
		public string Username { get; set; }
		
		public string ApiKey { get; set; }
		
		private LoginToken Token { get; set; }

		public Scanner()
		{

		}

		public Scanner(string username, string APIKey)
		{
			this.Token = UsersAuthentication.Login(username, APIKey); // This security token can be use multiple times, until it will be expired (48 hours).
		}

		public ResultRecord[] Scan(Uri url)
		{
			return Scan(url.AbsoluteUri);
		}

		public ResultRecord[] Scan(FileInfo file)
		{
			if (!file.Exists)
				throw new FileNotFoundException("File not found!", file.FullName);

			return Scan(file.FullName);
		}

		private ResultRecord[] Scan(string url)
		{
			// Create a new process on server.
			Detector detector = new Detector(this.Token);
			ScannerProcess process = detector.CreateProcess(url);

			// Waiting to process to be finished.
			while (!process.IsCompleted())
				Thread.Sleep(1000);

			// Getting results.
			return process.GetResults();
		}
	}
}
