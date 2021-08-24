

using System;
using System.Collections.Generic;

namespace Covid.Vaccination.SlotFinder
{
	public class VaccinationSession
	{
		    // need to change property naming convention with json property attribute
			public Guid session_id { get; set; }
		    public string date { get; set; }
		    public int available_capacity { get; set; }
	        public int min_age_limit { get; set; }
		    public string vaccine { get; set; }
		    public List<string> slots { get; set; }
		    public int available_capacity_dose1 { get; set; }
		    public int available_capacity_dose2 { get; set; }
	}
}
