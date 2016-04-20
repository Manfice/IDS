using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using IndDev.Auth.Model;
using IndDev.Domain.Abstract;
using IndDev.Models;

namespace IndDev.Domain.Context
{
    public class MailRepository : IMailRepository
    {
        private async Task<string> SendMyMailAsync(string body, string to, string subject)
        {
            var fromAddress = new MailAddress("manfice@gmail.com", "Industrial Development");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "1q2w3eQW")
            };
            using (var message = new MailMessage(fromAddress, new MailAddress(to))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                //message.To.Add(new MailAddress("ka.id@yandex.ru"));
                await smtp.SendMailAsync(message);
            }
            return "Message deliveryed";
        }

        public async Task<string> RegisterLetterAsync(string body, RegisterViewModel model)
        {
            var bd = body.Replace("{0}", model.Email);
            bd = bd.Replace("{1}", model.Password);
            return await SendMyMailAsync(bd, model.Email, "Спасибо за регистрацию на сайте www.id-racks.ru");
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