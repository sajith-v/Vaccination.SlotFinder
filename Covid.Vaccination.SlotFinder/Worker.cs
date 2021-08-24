using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Covid.Vaccination.SlotFinder
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly ISlotNotifier _slotNotifier;
		protected readonly Config _config;

		public Worker(ILogger<Worker> logger, ISlotNotifier slotNotifier, IOptions<Config> config)
		{
			_logger = logger;
			_slotNotifier = slotNotifier;
			_config = config.Value;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var interval = _config.IntervalInMinutes > 0 ? _config.IntervalInMinutes * 60 * 1000 : 5 * 60 * 1000; 
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await _slotNotifier.SendSlotNotification();
				await Task.Delay(interval, stoppingToken);
			}
		}
	}
}
