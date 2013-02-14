using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.InMemory.Tests
{
    [TestFixture]
    public class InMemoryPostCategoryRepositoryIntegrationFixture : IPostCategoryRepositoryFixture
    {
        private InMemoryStore _offlineStore;

        [SetUp]
        public void SetUp()
        {
            _offlineStore = new InMemoryStore();
        }

        protected override IPostCategoryRepository CreateIPostCategoryRepository()
        {
            return new InMemoryPostCategoryRepository(_offlineStore);
        }
    }
}
