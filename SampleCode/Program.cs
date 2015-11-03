using System;
using System.IO;
using Copyleaks.SDK.API.Exceptions;
using Copyleaks.SDK.API.Models;

namespace Copyleaks.SDK.SampleCode
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Usage:
			// SampleCode.exe -u "Your Account Username" -k "Your Account Key" --url "http://site.com/your-webpage"
			// OR 
			// SampleCode.exe -u "Your Account Username" -k "Your Account Key" --local-document "C:\your-directory\document.doc"

			CommandLineOptions options = new CommandLineOptions();
			if (!CommandLine.Parser.Default.ParseArguments(args, options))
				Environment.Exit(1);
			else if ((options.URL == null && options.LocalFile == null) || (options.URL != null && options.LocalFile != null))
			{
				Console.WriteLine("Error: You can speicfy either a URL or a local document to scan. For more information please enter '--help'.");
				Environment.Exit(1);
			}

			// For more information, visit Copyleaks "How-To page": https://api.copyleaks.com/Guides/HowToUse
			// Creating Copyleaks account: https://copyleaks.com/Account/Signup
			// Use your Copyleaks account information.
			// Generate your Account API Key: https://copyleaks.com/Account/Manage

			Scanner scanner = new Scanner(options.Username, options.ApiKey);
			uint creditsBalance = scanner.Credits;
			if (creditsBalance == 0)
			{
				Console.WriteLine("ERROR: You do not have enough credits to complete this scan. Your current credits balance = {0}).", creditsBalance);
				Environment.Exit(2);
			}

			Uri httpCallback = null;
			if (options.HttpCallback != null)
			{
				// Http callback example value:
				// https://your-website.com/copyleaks-gateway?id={PID}
				// Copyleaks server will replace the "{PID}" token with the actual process id.
				if (!Uri.TryCreate(options.HttpCallback, UriKind.Absolute, out httpCallback))
				{
					Console.WriteLine("ERROR: Bad Http-Callback.");
					Environment.Exit(3);
				}
			}

			try
			{
				ResultRecord[] results;
				if (options.URL != null)
				{
					Uri uri;
					if (!Uri.TryCreate(options.URL, UriKind.Absolute, out uri))
					{
						Console.WriteLine("ERROR: The URL ('{0}') is invalid.", options.URL); // Bad URL format.
						Environment.Exit(1);
					}

					results = scanner.ScanUrl(uri, httpCallback);
                }
				else
				{
					if (!File.Exists(options.LocalFile))
					{
						Console.WriteLine("ERROR: The file '{0}' does not exist.", options.LocalFile); // Bad URL format.
						Environment.Exit(1);
					}

					results = scanner.ScanLocalTextualFile(new FileInfo(options.LocalFile), httpCallback);
				}

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
						Console.WriteLine("Url: {0}", results[i].URL);
						Console.WriteLine("Precents: {0}", results[i].Precents);
						Console.WriteLine("CopiedWords: {0}", results[i].NumberOfCopiedWords);
					}
				}
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

			Environment.Exit(0);
		}
	}
}
