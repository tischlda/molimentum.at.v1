using System.Collections.Generic;
using Molimentum.Tasks.MailClient.Configuration;
using System;

namespace Molimentum.Tasks.MailClient
{
    public class MailClientTask : ITask
    {
        private readonly IMailService _mailService;
        private readonly Dictionary<string, IMessageProcessor> _messageProcessors = new Dictionary<string,IMessageProcessor>();

        public MailClientTask(IMailService mailService, IMessageProcessor[] messageProcessors)
        {
            _mailService = mailService;
            
            foreach(var messageProcessor in messageProcessors)
            {
                _messageProcessors.Add(messageProcessor.SupportedMessageCategory, messageProcessor);
            }
        }

        public void Execute()
        {
            try
            {
                _mailService.Connect();

                while (_mailService.HasMessages)
                {
                    var message = _mailService.FetchNext();

                    var parsedMessage = new ParsedMessage(message.From, message.To, message.Subject, message.Body, message.Attachments);

                    if (parsedMessage["KEY"] == MailClientConfiguration.Settings.Secret.Key)
                    {
                        if (_messageProcessors.ContainsKey(parsedMessage.Category))
                        {
                            _messageProcessors[parsedMessage.Category].ProcessMessage(parsedMessage);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var log = Elmah.ErrorLog.GetDefault(null);
                //log.ApplicationName = "molimentum.at";
                log.Log(new Elmah.Error(e));
            }
            finally
            {
                _mailService.Disconnect();
            }
        }
    }
}
