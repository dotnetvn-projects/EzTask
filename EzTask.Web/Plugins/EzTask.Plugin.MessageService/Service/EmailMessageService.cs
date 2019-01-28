using EzTask.Interface;
using EzTask.Plugin.MessageService.Data;
using System.Net;
using System.Net.Mail;
namespace EzTask.Plugin.MessageService.Service
{
    public class EmailMessageService : IMessageService<Message>
    {
        public EmailMessageService()
        {
            FrameworkCore.InvokeComponents()
        }

        public void Delivery(Message email)
        {
            Do(email as EmailMessage);
        }

        private void Do(EmailMessage email)
        {
            var fromAddress = new MailAddress(email.Sender, "EzTask");
            var toAddress = new MailAddress(email.To, email.To);
            string fromPassword = email.SenderPassword;
            string subject = email.Title;
            string body = email.Content;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
