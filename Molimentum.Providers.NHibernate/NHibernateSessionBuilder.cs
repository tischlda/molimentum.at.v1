using NHibernate;

namespace Molimentum.Providers.NHibernate
{
    public class NHibernateSessionBuilder : ISessionBuilder
    {
        private ISession _session;

        public ISession GetSession()
        {
            if(_session == null)
            {
                _session = NHibernateHelper.OpenSession();
            }

            return _session;
        }

        public void Dispose()
        {
            if(_session != null) _session.Dispose();
        }
    }
}