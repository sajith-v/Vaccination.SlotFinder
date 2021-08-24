

using System.Collections.Generic;

namespace Covid.Vaccination.SlotFinder
{
	public class EmailMessage
	{
		public List<string> ToAddress { get; set; }
		public string FromAddress { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public bool IsHtml { get; set; }
		public int Port { get; set; }
		public string Password { get; set; }
		public string Host { get; set; }
	}
}
