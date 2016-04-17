using IndDev.Models;

namespace IndDev.Domain.Abstract
{
    public interface IMailRepository
    {
        void SendMessage(MailMessageModel model, string body);
        string MakeLetterFromIndex(MailMessageModel message);
    }
}