

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Covid.Vaccination.SlotFinder
{
	public abstract class SlotNotifier
	{
        protected readonly Config _config;
        protected SlotNotifier(IOptions<Config> config)
        {

            _config = config.Value;
        }
        protected virtual async Task<List<Center>> FindSlotsByPincode()
		{
            var slotResponse = new SlotResponse();
            var configuredPincodes = _config.Pincodes;
			foreach (var pinCode in configuredPincodes)
			{
                using (var client = new HttpClient())
                {
                    var utcdate = DateTime.UtcNow.AddDays(1);
                    var date = TimeZoneInfo.ConvertTimeFromUtc(utcdate,
                                TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy");

                    // gives vaccines slot for umcoming dates if date is future
                    var url = $"{_config.SearchURL}/calendarByPin?pincode={pinCode}&date={date}";
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Empty);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var slot = JsonConvert.DeserializeObject<SlotResponse>(data);
                        if(slot != null && slot.centers != null && slot.centers.Any())
						{
                            slotResponse.centers.AddRange(slot.centers);
						}
                        Console.WriteLine(slotResponse);
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }

            var centers = FilterVaccineCenters(slotResponse.centers);
           
            return centers;
        }

        protected virtual async Task<List<Center>> FindVaccinationSlotsByDistrict()
		{
            var slotResponse = new SlotResponse();
            var districId = _config.DistricId;
            using (var client = new HttpClient())
            {
                var utcdate = DateTime.UtcNow.AddDays(1);
                var date = TimeZoneInfo.ConvertTimeFromUtc(utcdate, 
                            TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy");

                // gives vaccines slot for umcoming dates if date is future
                var url = $"{_config.SearchURL}/calendarByDistrict?district_id={districId}&date={date}";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(string.Empty);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var slot = JsonConvert.DeserializeObject<SlotResponse>(data);
                    if (slot != null && slot.centers != null && slot.centers.Any())
                    {
                        slotResponse.centers.AddRange(slot.centers);
                    }
                    Console.WriteLine(slotResponse);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }

            var centers = FilterVaccineCenters(slotResponse.centers);

            return centers;
        }

        private List<Center> FilterVaccineCenters(List<Center> centers)
		{
            if(centers != null && centers.Any())
			{
                if(_config.AgeLimit > 0)
				{
                    centers = centers.Select(x =>
                    {
                        x.sessions = x.sessions.Where(a => a.min_age_limit >= _config.AgeLimit).ToList();
                        return x;
                    }).ToList();
				}

                if (!string.IsNullOrWhiteSpace(_config.Vaccine))
                {
                    centers = centers.Select(x =>
                    {
                        x.sessions = x.sessions.Where(a => a.vaccine.ToLower() == _config.Vaccine.ToLower()).ToList();
                        return x;
                    }).ToList();
                }

                centers = centers.Select(x =>
                {
                    x.sessions = x.sessions.Where(a => a.available_capacity_dose1 >0 || a.available_capacity_dose2 > 0 || a.available_capacity > 0).ToList();
                    return x;
                }).ToList();

            }

            centers = centers.Where(x => x.sessions.Any()).ToList();
            return centers;
		}
	}
}
