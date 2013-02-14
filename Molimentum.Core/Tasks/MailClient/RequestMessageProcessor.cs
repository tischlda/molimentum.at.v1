using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Molimentum.Tasks.MailClient
{
    public class RequestMessageProcessor : MessageProcessorBase
    {
        public RequestMessageProcessor()
            : base("REQUEST")
        {
        }

        public override void ProcessMessage(ParsedMessage parsedMessage)
        {
            base.ProcessMessage(parsedMessage);

            foreach (var line in parsedMessage.Body.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var replyBuilder = new StringBuilder();

                var request = (HttpWebRequest)WebRequest.Create(line);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    replyBuilder.AppendFormat("Response Uri: {0}{1}", response.ResponseUri, Environment.NewLine);
                    replyBuilder.AppendFormat("Status Code: {0}{1}", response.StatusCode, Environment.NewLine);
                    replyBuilder.AppendFormat("Last Modified: {0}{1}", response.LastModified, Environment.NewLine);
                    replyBuilder.AppendLine();

                    using (var responseStreamReader = new StreamReader(response.GetResponseStream()))
                    {
                        replyBuilder.Append(responseStreamReader.ReadToEnd());
                    }
                }

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Send("blog@molimentum.at", parsedMessage.From, line, replyBuilder.ToString());
                }
            }
        }
    }
}