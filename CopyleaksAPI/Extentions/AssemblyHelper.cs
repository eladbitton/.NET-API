using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Copyleaks.SDK.API.Extentions
{
	public static class AssemblyHelper
	{
		public static string GetVersion(this Assembly asm)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			return fvi.FileVersion;
		}
	}
}
