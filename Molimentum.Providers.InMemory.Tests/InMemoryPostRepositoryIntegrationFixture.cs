using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.InMemory.Tests
{
    [TestFixture]
    public class InMemoryPostRepositoryIntegrationFixture : IPostRepositoryFixture
    {
        private InMemoryStore _offlineStore;

        [SetUp]
        public void SetUp()
        {
            _offlineStore = new InMemoryStore();
        }

        protected override IPostRepository CreateIPostRepository()
        {
            return new InMemoryPostRepository(_offlineStore);
        }
    }
}
