using System.Threading.Tasks;
using IndDev.Auth.Model;
using IndDev.Models;

namespace IndDev.Domain.Abstract
{
    public interface IMailRepository
    {
        void SendMessage(MailMessageModel model, string body);
        Task<string> RegisterLetterAsync(string body, RegisterViewModel model);
        Task<string> ResetPassword(string body, string to, string resetLink);

    }
}