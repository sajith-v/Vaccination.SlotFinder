using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid.Vaccination.SlotFinder
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					services.Configure<Config>(hostContext.Configuration.GetSection("Configurations"));
					services.AddHostedService<Worker>();
					services.AddTransient<Config>(_ => _.GetRequiredService<IOptions<Config>>().Value);
					services.AddTransient<ISlotNotifier, EmailSlotNotifier>();
				});
	}
}
