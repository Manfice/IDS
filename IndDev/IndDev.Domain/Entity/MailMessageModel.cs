using System.Web;

namespace IndDev.Models
{
    public class MailMessageModel
    {
        public string SenderName { get; set; } // имя отправителя
        public string From { get; set; }// мыло реципиента
        public string To { get; set; }//мыло для ммассовой рассылки
        public string Subject { get; set; }//тема
        public string Body { get; set; }//сообщение
        public HttpPostedFileBase Attachment { get; set; }//прикрепленные файлы
    }
}