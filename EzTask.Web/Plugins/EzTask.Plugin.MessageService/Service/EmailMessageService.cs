using EzTask.Interface;
using EzTask.Interface.SharedData;
using EzTask.Plugin.MessageService.Data;
using EzTask.Plugin.MessageService.Data.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EzTask.Plugin.MessageService.Service
{
    public class EmailMessageService : IMessageService<IMessage>
    {
        private IOptions<MailServerSettings> _mailSetting;
        public EmailMessageService()
        {
            _mailSetting = MessageServiceRegister.ServiceProvider
                .InvokeComponents<IOptions<MailServerSettings>>();
        }

        public void Delivery(IMessage email)
        {
            Task.Factory.StartNew(() =>
            {
                Do(email as EmailMessage);
            });  
        }

        private void Do(EmailMessage email)
        {
            try
            {
                var fromAddress = new MailAddress(_mailSetting.Value.AppEmail, "EzTask");
                var toAddress = new MailAddress(email.To, email.To);
                string fromPassword = _mailSetting.Value.AppEmailPass;
                string subject = email.Title;
                string body = email.Content;

                var smtp = new SmtpClient
                {
                    Host = _mailSetting.Value.Smtp,
                    Port = _mailSetting.Value.Port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Priority = MailPriority.High,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.Default,
                    Body = body,
                 })
                {
                  message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, new System.Net.Mime.ContentType("text/html")));
                  smtp.Send(message);
                }
            }
            catch
            {
                //Just ignore because sometime there is fake email
            }          
        }
    }
}
