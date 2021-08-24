

using System.Collections.Generic;

namespace Covid.Vaccination.SlotFinder
{
	public class Center
	{
		// need to change property naming convention with json property attribute
		public Center()
		{
			sessions = new List<VaccinationSession>();
		}
		public long center_id { get; set; }
		public string name { get; set; }
		public string address { get; set; }
		public string state_name { get; set; }
		public string district_name { get; set; }
		public string fee_type { get; set; }
		public List<VaccinationSession> sessions { get; set; }
	}
}
