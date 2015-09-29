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
			// For more information, visit Copyleaks How-To page: https://api.copyleaks.com/Guides/HowToUse

			// Creating Copyleaks account: https://copyleaks.com/Account/Signup
			// Use your account information:
			string username = "<Username>";
			string APIKey = "<API KEY>"; // Generate your API Key: https://copyleaks.com/Account/Manage

			Uri url_to_scan = new Uri("http://www.website.com/document-to-scan"); // Allowed formats: html, pdf, doc, docx, rtf, txt ...
			Scanner scanner = new Scanner(username, APIKey);
			try
			{
				var results = scanner.ScanUrl(url_to_scan);
				// Another scanning option --> scanner.ScanLocalTextualFile(file)

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
		}
	}
}
