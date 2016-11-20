namespace SocialContactScript
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;

    using Configuration;

    using SocialContactScript.Utilities;

    class Gmail
    {
        private const string EmailSubject = "[IMPORTANT] Entrepreneurship Offer";

        public void SendHtmlFormattedEmail(string htmlPath, IList<string> htmlVariables)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(SmtpConfig.UserMailId, SmtpConfig.Username);
                mailMessage.Subject = EmailSubject;
                mailMessage.Body = GetBody(htmlPath, htmlVariables.SkipLast());  //as last variable is emailId
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(htmlVariables.Last()));
                mailMessage.Priority = MailPriority.High;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                var smtp = new SmtpClient
                {
                    Host = SmtpConfig.Host,
                    Port = SmtpConfig.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = SmtpConfig.EnableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential
                                       {
                                           UserName = SmtpConfig.UserMailId,
                                           Password = SmtpConfig.Password
                                       }
                    
                };
                smtp.Send(mailMessage);
            }
        }

        private string GetBody(string htmlPath, IEnumerable<string> variables)
        {
            string body;
            using (var reader = new StreamReader(htmlPath))
            {
                body = reader.ReadToEnd();
            }

            body = string.Format(body, variables.ToArray());
            return body;
        }
    }
}
