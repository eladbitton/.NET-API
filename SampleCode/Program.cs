using System;
using System.Threading;
using Copyleaks.SDK.API;
using Copyleaks.SDK.API.Exceptions;

namespace Copyleaks.SDK.SampleCode
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Creating Copyleaks account: https://copyleaks.com/Account/Signup
			// Use your account information:
			string username = "<Username>";
			string APIKey = "<API KEY>"; // Generate your API Key: https://copyleaks.com/Account/Manage

			string url_to_scan = "http://www.website.com/document-to-scan"; // Allowed formats: html, pdf, doc, docx, rtf, txt ...

			try
			{
				Scan(username, APIKey, url_to_scan);
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("\tFailed!");
				Console.WriteLine("+Error Description:");
				Console.WriteLine("{0}", e.Message);
			}
			catch (CommandFailedException theError)
			{
				Console.WriteLine("\tFailed!");
				Console.WriteLine("+Error Description:");
				Console.WriteLine("{0}", theError.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("\tFailed!");
				Console.WriteLine("Unhandled Exception");
				Console.WriteLine(ex);
			}
		}

		public static void Scan(string username, string apiKey, string url)
		{
			// Login to Copyleaks server.
			Console.Write("User login... ");
			LoginToken token = UsersAuthentication.Login(username, apiKey);
			// This security token can be use multiple times, untill it will be expired (14 days).
			Console.WriteLine("\t\t\tSuccess!");

			// Create a new process on server.
			Console.Write("Submiting new request... ");
			Detector detector = new Detector(token);
			ScannerProcess process = detector.CreateProcess(url);
			Console.WriteLine("\tSuccess!");

			// Waiting to process to be finished.
			Console.Write("Waiting for completion... ");
			while (!process.IsCompleted())
				Thread.Sleep(1000);
			Console.WriteLine("\tSuccess!");

			// Getting results.
			Console.Write("Getting results... ");
			var results = process.GetResults();
			if (results.Length == 0)
			{
				Console.WriteLine("\tNo results.");
			}
			else
			{
				for (int i = 0; i < results.Length; ++i)
				{
					Console.WriteLine();
					Console.WriteLine("Result {0}:", i + 1);
					Console.WriteLine("Domain: {0}", results[i].Domain);
					Console.WriteLine("Url: {0}", results[i].URL);
					Console.WriteLine("Precents: {0}", results[i].Precents);
					Console.WriteLine("CopiedWords: {0}", results[i].NumberOfCopiedWords);
				}
			}
		}
	}
}
