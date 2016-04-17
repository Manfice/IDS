using System;
using System.Net;
using System.Net.Mail;
using IndDev.Domain.Abstract;
using IndDev.Models;

namespace IndDev.Domain.Context
{
    public class MailRepository : IMailRepository
    {
        public string MakeLetterFromIndex(MailMessageModel message)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(MailMessageModel model, string body)
        {
            var fromAddress = new MailAddress("manfice@gmail.com", "Industrial Development");
            var toAddress = new MailAddress(model.From, "Customer");
            const string fromPassword = "1q2w3eQW";
            const string subject = "Отправленно с сайта, с главной формы";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var bd = body.Replace("{0}", model.SenderName);
            bd = bd.Replace("{1}", DateTime.Now.ToString("f"));
            bd = bd.Replace("{2}", model.Body);
            bd = bd.Replace("{3}", model.From);
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = bd,
                IsBodyHtml = true
            })
            {
                message.To.Add(new MailAddress("ka.id@yandex.ru"));
                smtp.Send(message);
            }
        }
    }
}