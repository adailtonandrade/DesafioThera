using SendGrid;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid.Helpers.Mail;
using System.Configuration;

namespace Identity.Configuration
{
    public class MailService : IIdentityMessageService
    {

        public Task SendAsync(IdentityMessage message)
        {
            return ConfigSendGridasync(message);
        }

        // Implementação do SendGrid
        private Task ConfigSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.SetFrom(new EmailAddress("admin@portal.com.br", "Admin do Portal"));
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.Body;
            myMessage.HtmlContent = message.Body;

            var response = SendAsync(myMessage).Result;
            if (!response.IsSuccessStatusCode)
            {
                return Task.FromResult(response);
            }
            else
            {
                return Task.FromResult(0);
            }
        }
        private async Task<Response> SendAsync(SendGridMessage message)
        {
            var client = new SendGridClient(ConfigurationManager.AppSettings["mailApi"]);
            return await client.SendEmailAsync(message);
        }
        //public Task SendAsync(IdentityMessage message)
        //{
        //    var mail = _mail;
        //    var password = _password;
        //    var smtp = _SMTP;
        //    var port = _port;
        //    var displayName = _displayName;
        //    var path = string.Empty; //System.Web.HttpContext.Current.Server.MapPath("~/Metronic/assets/layouts/layout3/img/logo-default.png");
        //    MailModel mailModel = new MailModel(mail, password, smtp, port, displayName, path);
        //    mailModel.Body = message.Body;
        //    mailModel.Subject = message.Subject;
        //    mailModel.To = new List<MailAddress>() { new MailAddress(message.Destination) };
        //    mailModel.From = new MailAddress(mail, displayName);
        //    Domain.Util.MailService.SendEmail(mailModel);
        //    return Task.FromResult(0);
        //}
        //public Task SendAsync(ContactVM contact)
        //{
        //    var path = string.Empty; // System.Web.HttpContext.Current.Server.MapPath("~/Metronic/assets/layouts/layout3/img/logo-default.png");
        //    MailModel mailModel = new MailModel(_mail, _password, _SMTP, _port, contact.EmitterName, path);
        //    mailModel.Body = contact.Body;
        //    mailModel.Subject = contact.Subject;
        //    mailModel.To = new List<MailAddress>() { new MailAddress(contact.To) };
        //    mailModel.From = new MailAddress(contact.From, contact.EmitterName);
        //    Domain.Util.MailService.SendEmail(mailModel);
        //    return Task.FromResult(0);
        //}
    }
}