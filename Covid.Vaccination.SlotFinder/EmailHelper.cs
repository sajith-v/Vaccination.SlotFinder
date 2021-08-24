using System;
using System.Net;
using System.Net.Mail;

namespace Covid.Vaccination.SlotFinder
{
	public static class EmailHelper
	{
        public static void Email(EmailMessage email)
        {
            try
            {
                var message = new MailMessage();
                var smtp = new SmtpClient();
                message.From = new MailAddress(email.FromAddress);
				foreach (var to in email.ToAddress)
				{
                    message.To.Add(new MailAddress(to));
                }
                message.Subject = email.Subject;
                message.IsBodyHtml = email.IsHtml;
                message.Body = email.Body;
                smtp.Port = email.Port;
                smtp.Host = email.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(email.FromAddress, email.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
