using System.Threading.Tasks;
using IndDev.Auth.Model;
using IndDev.Domain.Entity.Orders;
using IndDev.Models;

namespace IndDev.Domain.Abstract
{
    public interface IMailRepository
    {
        Task<string> RegisterLetterAsync(string body, RegisterViewModel model);
        Task<string> ResetPassword(string body, string to, string resetLink);
        Task<string> MessageFromTitle(string body, MailMessageModel model);
        Task<string> OrderNotify(Order order,string body);
    }
}