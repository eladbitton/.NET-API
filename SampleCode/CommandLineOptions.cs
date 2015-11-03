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

		[Option("url", Required = false, HelpText = "URL to scan")]
		public string URL { get; set; }

		[Option("local-document", Required = false, HelpText = "Local file to upload. Allowed formats - txt, rtf, doc, docx, pdf")]
		public string LocalFile { get; set; }

		[Option("http-callback", Required = false, HelpText = "Http-Callback that will be invoked once the scan is done.")]
		public string HttpCallback { get; set; }

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
