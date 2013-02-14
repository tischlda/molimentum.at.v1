using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Molimentum.Model;
using Molimentum.Services;
using System.Net.Mail;
using Molimentum.Services.Configuration;

namespace Molimentum.Services
{
    public class EMailNotificationService : INotificationService
    {
        public void Notify(string action, object o)
        {
            using (var client = new SmtpClient())
            {
                var message = new MailMessage(
                    MvcConfiguration.Settings.EMailNotification.From,
                    MvcConfiguration.Settings.EMailNotification.To,
                    "NOTIFICATION: " + action,
                    o.ToString());

                client.Send(message);
            }
        }
    }
}