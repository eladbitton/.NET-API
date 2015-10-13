using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace Copyleaks.SDK.SampleCode
{
	public class CommandLineOptions
	{
		[Option('u', "username", Required = true, HelpText = "Copyleaks account username")]
		public string Username { get; set; }

		[Option('k', "key", Required = true, HelpText = "Copyleaks account API key")]
		public string ApiKey { get; set; }

		[Option("url", Required = false, HelpText = "URL for scanning")]
		public string URL { get; set; }

		[Option("local-document", Required = false, HelpText = "Local document file to upload. Allowed formats (txt, rtf, doc, docx, pdf, ...).")]
		public string LocalFile { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
			  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
