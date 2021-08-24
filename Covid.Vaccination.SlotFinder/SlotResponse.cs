

using System.Collections.Generic;

namespace Covid.Vaccination.SlotFinder
{
	public class SlotResponse
	{
		// need to change property naming convention with json property attribute
		public SlotResponse()
		{
			centers = new List<Center>();
		}
		public List<Center> centers { get; set; }
	}
}
