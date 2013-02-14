using System;
using NHibernate;

namespace Molimentum.Providers.NHibernate
{
    public interface ISessionBuilder : IDisposable
    {
        ISession GetSession();
    }
}