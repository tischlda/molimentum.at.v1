using System;

namespace Molimentum.Providers.InMemory
{
    public abstract class InMemoryRepositoryBase : IDisposable
    {
        protected IStore Store { get; private set; }

        public InMemoryRepositoryBase(IStore store)
        {
            Store = store;
        }

        public virtual void Dispose()
        {

        }
    }
}
