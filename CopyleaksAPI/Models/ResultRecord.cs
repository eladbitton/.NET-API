using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Copyleaks.SDK.API.Models
{
	public class ResultRecord
	{
		[JsonProperty(PropertyName = "Domain")]
		public string Domain { get; private set; }

		[JsonProperty(PropertyName = "URL")]
		public string URL { get; private set; }

		[JsonProperty(PropertyName = "Precents")]
		public int Precents { get; private set; }

		[JsonProperty(PropertyName = "NumberOfCopiedWords")]
		public int NumberOfCopiedWords { get; set; }
	}
}
