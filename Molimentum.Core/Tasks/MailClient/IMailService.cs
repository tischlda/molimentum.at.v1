using System;

namespace Molimentum.Tasks.MailClient
{
    public interface IMailService : IDisposable
    {
        void Connect();
        void Disconnect();

        bool HasMessages { get;  }
        MailMessage FetchNext();
    }
}
