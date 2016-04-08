using IndDev.Models;

namespace IndDev.Domain.Abstract
{
    public interface IMailRepository
    {
        void SendMessage(MailMessageModel model);
    }
}