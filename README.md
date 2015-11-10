<h2>Copyleaks SDK</h2>
<p>
Copyleaks SDK is a simple framework that allows you to perform plagiarism scans and track content distribution around the web, using Copyleaks cloud.
</p>
<p>
With Copyleaks SDK you can submit for scan:  
<ul>
<li>Webpages</li>
<li>Local files (see supported files here)</li>
<li>OCR (Optical Character Recognition) - scanning pictures containing textual content (see supported files here)</li>
</ul>
Instructions for using the SDK are below. For a quick example demonstrating the SDK capabilities just look at the code examples under “examples”.
</p>
<h3>Configuration</h3>
<p>You can configure Copyleaks SDK in one of two ways:</p>
<ol>
<li>Download the code from here, compile it and add reference to the assembly.</li>
<li>Add <i>CopyleaksAPI</i> NuGet by running the following command in the <a href="http://docs.nuget.org/consume/package-manager-console">Package Manager Console</a></li>
<pre>
Install-Package CopyleaksAPI
</pre>
</ol>
<h3>Signing Up and Getting Your API Key</h3>
 <p>To use Copyleaks API you need to be a registered user. Signing up is free of charge.</p>
 <p><a href="https://copyleaks.com/Account/Signup">Signup</a> to Copyleaks and confirm your account by clicking the link on the confirmation email. To get your API key, click your username in the upper right corner which will lead you to <i>‘Manage Account’</i> page. Press on <i>"Generate"</i> to get your own personal API key.</p>
 <p>For more information check out our <a href="https://api.copyleaks.com/Guides/HowToUse">API guide</a>.</p>
<h3>Example</h3>
<p>This code will show you where the textual content in the parameter ‘url’ has been used online:</p>
<pre>
using System;
using System.Threading;
using Copyleaks.SDK.API;
// ...

private static void Scan(string username, string apiKey, string url)
{
    Console.Write("User login... ");
    LoginToken token = UsersAuthentication.Login(username, apiKey);// Login to Copyleaks server. This security token can be use multiple times, untill it will be expired (14 days).
    Console.WriteLine("\t\t\tSuccess!");


    Console.Write("Submiting new request... ");
    Detector detector = new Detector(token);
    ScannerProcess process = detector.CreateByUrl(new Uri(url));// Create a new process on server.
    Console.WriteLine("\tSuccess!");


    Console.Write("Waiting for completion... ");
        while (!process.IsCompleted())
        Thread.Sleep(1000);// Waiting to process to be finished.
    Console.WriteLine("\tSuccess!");


    Console.Write("Getting results... ");
        var results = process.GetResults();// Getting results.
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
</pre>
<h3>Dependencies:</h3>
<ul>
<li><a href="http://www.microsoft.com/en-us/download/details.aspx?id=30653">.Net framework 4.5</a></li>
</ul>
<h5>Referenced Assemblies:</h5>
<ul>
<li><a href="https://www.nuget.org/packages/Microsoft.Net.Http">Microsoft.Net.Http</a></li>
<li><a href="https://www.nuget.org/packages/Newtonsoft.Json">Newtonsoft.Json</a></li>
<li><a href="https://www.nuget.org/packages/Microsoft.Bcl">Microsoft.Bcl</a></li>
<li><a href="https://www.nuget.org/packages/Microsoft.Bcl.Build/1.0.21">Microsoft.Bcl.Build</a></li>
</ul>

<h3>Read More</h3>
<ul>
<li><a href="https://api.copyleaks.com/Guides/HowToUse">Copyleaks API guide</a></li>
<li><a href="https://api.copyleaks.com/Help">Copyleaks API documentation</a></li>
<li><a href="https://www.nuget.org/packages/CopyleaksAPI/">Copyleaks NuGet package</a></li>
</ul>
