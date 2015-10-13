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
				Console.WriteLine("Error: Exactly one input must be specified: 'url' or 'local-document'. Use '--help' for more information.");
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
				Console.WriteLine("ERROR: You have insufficient credits for scanning content! (current credits balance = {0})", creditsBalance);
				Environment.Exit(2);
			}

			try
			{
				ResultRecord[] results;
				if (options.URL != null)
				{
					Uri uri;
					if (!Uri.TryCreate(options.URL, UriKind.Absolute, out uri))
					{
						Console.WriteLine("ERROR: The URL ('{0}') is invalid!", options.URL); // Bad URL format.
						Environment.Exit(1);
					}

					results = scanner.ScanUrl(uri);
				}
				else
				{
					if (!File.Exists(options.LocalFile))
					{
						Console.WriteLine("ERROR: The file '{0}' is not exists!", options.LocalFile); // Bad URL format.
						Environment.Exit(1);
					}

					results = scanner.ScanLocalTextualFile(new FileInfo(options.LocalFile));
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
						Console.WriteLine("Domain: {0}", results[i].Domain);
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
