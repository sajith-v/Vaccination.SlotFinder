

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Covid.Vaccination.SlotFinder
{
	public interface ISlotNotifier
	{
		Task SendSlotNotification();
	}
}
