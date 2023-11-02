using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace OnlineShop.Core.Senders
{
    public class SendEmail
    {
        public static async Task<bool> SendAsync(string to, string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    mail.From = new MailAddress("daneshvargroup.official@gmail.com", "دانشور");

                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    smtpClient.Credentials = new NetworkCredential("daneshvargroup.official@gmail.com", "iwvj nvvm bsmy hxan");
                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(mail);

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}