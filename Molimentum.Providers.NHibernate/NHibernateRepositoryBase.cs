using System;
using NHibernate;

namespace Molimentum.Providers.NHibernate
{
    public abstract class NHibernateRepositoryBase : IDisposable
    {
        protected ISession Session { get; private set; }

        public NHibernateRepositoryBase(ISessionBuilder sessionBuilder)
        {
            Session = sessionBuilder.GetSession();
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
                if (Session != null)
                {
                    Session.Dispose();
                    Session = null;
                }
           }

            _disposed = true;
        }

        ~NHibernateRepositoryBase()
        {
            Dispose(false);
        }
    }
}
