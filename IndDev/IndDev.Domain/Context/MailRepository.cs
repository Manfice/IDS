using System;
using System.Net;
using System.Net.Mail;
using IndDev.Domain.Abstract;
using IndDev.Models;

namespace IndDev.Domain.Context
{
    public class MailRepository : IMailRepository
    {
        public void SendMessage(MailMessageModel model)
        {
            var body = "<p>Wow {0} WOW {1}</p>";
            var smtp = new SmtpClient()
            {
                Credentials = new NetworkCredential("manfice@gmail.com", "1q2w3eOP"),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Port = 587,
                Host = "smtp.gmail.com",
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var message = new MailMessage()
            {
                Body = string.Format(body, model.SenderName, model.From),
                IsBodyHtml = true,
                Subject = "test",
                From = new MailAddress("manfice@gmail.com")
            };
            message.To.Add(model.From);
            try
            {
                smtp.Send(message);
            }
            catch (Exception x)
            {
                throw ;
            }


        }
    }
}