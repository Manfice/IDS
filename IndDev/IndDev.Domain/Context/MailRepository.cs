using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;
using IndDev.Auth.Model;
using IndDev.Domain.Abstract;
using IndDev.Domain.Entity.Auth;
using IndDev.Models;

namespace IndDev.Domain.Context
{
    public class MailRepository : IMailRepository
    {
        
        private async Task<string> SendMyMailAsync(string body, string to, string subject)
        {
            var fromAddress = new MailAddress("admin@id-racks.ru", "Industrial Development");
            var smtp = new SmtpClient
            {
                Host = "smtp.yandex.ru",
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "1q2w3eZX")
            };
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, errors) => true;
            using (var message = new MailMessage(fromAddress, new MailAddress(to))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(message);
            }
            return $"Письмо отправленно на адрес: {to}";
        }

        public async Task<string> RegisterLetterAsync(string body, RegisterViewModel model)
        {
            var bd = body.Replace("{0}", model.Email);
            bd = bd.Replace("{1}", model.Password);
            var mails = new List<MailAddress>
            {
                new MailAddress("c592@yandex.ru")
            };
            foreach (var address in mails)
            {
                await SendMyMailAsync(bd, address.Address, "Регистрация нового пользователя");
            }
            return await SendMyMailAsync(bd, model.Email, "Спасибо за регистрацию на сайте www.id-racks.ru");
        }

        public async Task<string> MessageFromTitle(string body, MailMessageModel model)
        {
            body = body.Replace("{0}", model.SenderName);
            body = body.Replace("{1}", DateTime.Today.ToShortDateString());
            body = body.Replace("{3}", model.From);
            body = body.Replace("{2}", model.Body);
            var mails = new List<MailAddress>
            {
                new MailAddress("ka.id@yandex.ru"),
                new MailAddress("c592@yandex.ru")
            };
            foreach (var address in mails)
            {
                await SendMyMailAsync(body, address.Address, "Вопрос с сайта");
            }
            return await SendMyMailAsync(body, model.From, "Вопрос с сайта www.id-racks.ru");
        }

        public async Task<string> ResetPassword(string body, string to, string resetLink)
        {
            using (var context = new DataContext())
            {
                var usr = context.Users.FirstOrDefault(user => user.EMail==to);
                if (usr!=null)
                {
                    var bd = body.Replace("{0}", usr.Name);
                    bd = bd.Replace("{1}", to);
                    bd = bd.Replace("{2}", resetLink);
                    return await SendMyMailAsync(bd, to, "Сброс пароля.");
                }
                return $"Пользователь с логином {to} не найден в бд.";
            } 

        }
    }
}