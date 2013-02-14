using System;
using System.Collections.Generic;
using LumiSoft.Net.Mail;
using LumiSoft.Net.Mime;
using LumiSoft.Net.MIME;
using LumiSoft.Net.POP3.Client;
using Molimentum.Tasks.MailClient.PopMailProvider.Configuration;

namespace Molimentum.Tasks.MailClient.PopMailProvider
{
    public class PopMailService : IMailService
    {
        private POP3_Client _popClient;

        public void Connect()
        {
            if (_popClient != null) throw new ApplicationException("Already connected.");

            _popClient = new POP3_Client();
            _popClient.Connect(PopMailProviderConfiguration.Settings.Server.Name, PopMailProviderConfiguration.Settings.Server.Port);
            _popClient.Authenticate(PopMailProviderConfiguration.Settings.Server.Username, PopMailProviderConfiguration.Settings.Server.Password, true);
        }

        public void Disconnect()
        {
            if (_popClient != null)
            {
                _popClient.Dispose();
                _popClient = null;
            }
        }

        public bool HasMessages
        {
            get
            {
                EnsureConnection();

                return _popClient.Messages.Count > 0;
            }
        }

        public MailMessage FetchNext()
        {
            EnsureConnection();

            if (_popClient.Messages.Count == 0) throw new ApplicationException("No messages.");

            var mailMessage = Mail_Message.ParseFromByte(_popClient.Messages[0].MessageToByte());

            _popClient.Messages[0].MarkForDeletion();

            return new MailMessage(mailMessage.From[0].Address, mailMessage.To.Mailboxes[0].Address, mailMessage.Subject, mailMessage.BodyHtmlText ?? mailMessage.BodyText, GetAttachments(mailMessage.Attachments));
        }

        private static IEnumerable<MailAttachment> GetAttachments(IEnumerable<MIME_Entity> attachments)
        {
            foreach (var attachment in attachments)
            {
                var parsedMime = Mime.Parse(attachment.ToByte(null, null));

                yield return new MailAttachment(attachment.ContentDisposition.Param_FileName, parsedMime.MainEntity.Data);
            }
        }

        private void EnsureConnection()
        {
            if (_popClient == null)
            {
                throw new ApplicationException("Not connected.");
            }
        }


        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // release managed resources
                }

                // release unmanaged resources
                // disposal of injected dependencies is managed by the IOC container
                if (_popClient != null)
                {
                    _popClient.Dispose();
                    _popClient = null;
                }
           }

            _disposed = true;
        }

        ~PopMailService()
        {
            Dispose(false);
        }
    }
}