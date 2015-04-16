using System;
using System.Threading;
using System.Threading.Tasks;
using Copyleaks.SDK.API;
using Copyleaks.SDK.API.Models;

namespace SampleCode
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] URLs = new string[]{
					"http://www.url.com/Your_page",
					"http://www.url.com/Your_page2",
					"http://www.url.com/Your_page3"
				};
			LoginToken token;
			try
			{
				token = Login("<YOUR_USER_NAME>", "<YOUR_API_KEY>");
				foreach (var url in URLs)
				{
					Scan(token, url);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception");
				Console.WriteLine(ex);
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		private static LoginToken Login(string username, string apiKey)
		{
			Console.Write("Log-in to Copyleaks server...\t\t");
			LoginToken token = UsersAuthentication.Login(username, apiKey);
			Console.WriteLine("Done!");
			return token;
		}

		private static void Scan(LoginToken token, string url)
		{
			Detector detector;
			ScannerProcess process;

			Console.Write("Creating new process on server...\t");
			detector = new Detector(token);
			process = detector.CreateProcessAsync(url).Result;
			Console.WriteLine("Done!");

			Console.Write("Waiting for process completion...\t");
			while (!process.IsCompletedAsync().Result)
				Thread.Sleep(1000);
			Console.WriteLine("Done!");

			Console.Write("Getting resutls... ");
			ResultRecord[] results;
			results = process.GetResultsAsync().Result;

			if (results.Length == 0)
				Console.WriteLine("No results.");
			else
				for (int i = 0; i < results.Length; ++i)
				{
					Console.WriteLine();
					Console.WriteLine("Result {0}:", i + 1);
					Console.WriteLine("Domain: {0}", results[i].Domain);
					Console.WriteLine("Url: {0}", results[i].URL);
					Console.WriteLine("Precents: {0}", results[i].Precents);
					Console.WriteLine("CopiedWords: {0}", results[i].NumberOfCopiedWords);
				}

			Console.WriteLine();
			Console.Write("Deleting process '{0}'...\t", process.Id);
			process.DeleteAsync();
			Console.WriteLine("Done!");
		}
	}
}
