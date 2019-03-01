using System.Collections.Generic;

namespace DogsRUs.Classes
{
	public class JsonListResponse
	{
		public string status { get; set; }

		public List<string> message { get; set; }
	}

	public class JsonUrlResponse
	{
		public string status { get; set; }

		public string message { get; set; }
	}
}
