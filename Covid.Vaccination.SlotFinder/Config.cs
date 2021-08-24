

using System.Collections.Generic;

namespace Covid.Vaccination.SlotFinder
{
	public class Config
	{
		public string SearchURL { get; set; }
		public List<long> Pincodes { get; set; }
		public int AgeLimit { get; set; }
		public string Vaccine { get; set; }
		public string NotificationToEmail { get; set; }
		public string NotificationFromEmail { get; set; }
		public int EmailPort { get; set; }
		public string EmailPassword { get; set; }
		public string EmailHost { get; set; }
		public int DistricId { get; set; }
		public int IntervalInMinutes { get; set; }
	}
}
