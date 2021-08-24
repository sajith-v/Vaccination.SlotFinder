
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid.Vaccination.SlotFinder
{
	public class EmailSlotNotifier : SlotNotifier, ISlotNotifier
	{
		public EmailSlotNotifier(IOptions<Config> config):base(config)
		{

		}
		public async Task SendSlotNotification()
		{
			try
			{
				List<Center> centers = new List<Center>();
				if (_config.Pincodes != null && _config.Pincodes.Any())
				{
					centers = await base.FindSlotsByPincode();
				}
				else
				{
					centers = await base.FindVaccinationSlotsByDistrict();
				}

				if (centers != null && centers.Any())
				{
					var emailContent = @"<html><body><div><h4> Vaccine available for the following centers </h4>
									<ol>
									###emailbodycontent###
									</ol></div></body></html>";
					var emailBody = string.Empty;
					foreach (var item in centers)
					{
						var dates = string.Join(',', item.sessions.Where(x => x.date != null).Select(x=>x.date).Distinct().ToList());
						var center = $"<li><p> {item.name} - <span> {string.Join(',',item.sessions.Select(x=>x.vaccine))} - </span>  <span> {item.address}   </span> - <span>{dates} </span> </p></li>";
						emailBody = $"{emailBody} {center}";
					}
					emailContent = emailContent.Replace("###emailbodycontent###", emailBody);
					SendEmail(emailContent);
				}
			}
			catch (System.Exception ex)
			{

				Console.WriteLine(ex);
			}
			
        }

        private void SendEmail(string htmlString)
        {
			var emailMessage = new EmailMessage()
			{
				FromAddress = _config.NotificationFromEmail,
				ToAddress = new List<string>() { _config.NotificationToEmail },
				Body = htmlString,
				IsHtml = true,
				Host = _config.EmailHost,
				Password = _config.EmailPassword,
				Port = _config.EmailPort,
				Subject = "Vaccine slot available"
			};

			EmailHelper.Email(emailMessage);
        }

    }
}
