using System.Collections.Generic;

namespace Molimentum.Tasks.MailClient
{
    public class MailMessage
    {
        public MailMessage(string from, string to, string subject, string body, IEnumerable<MailAttachment> attachments)
        {
            Attachments = attachments;
            From = from;
            To = to;
            Subject = subject;
            Body = body;
        }

        public string From { get; private set; }
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public IEnumerable<MailAttachment> Attachments { get; private set; }
    }
}