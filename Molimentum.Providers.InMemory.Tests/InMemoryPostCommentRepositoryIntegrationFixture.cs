using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.InMemory.Tests
{
    [TestFixture]
    public class InMemoryPostCommentRepositoryIntegrationFixture : IPostCommentRepositoryFixture
    {
        private InMemoryStore _offlineStore;

        [SetUp]
        public void SetUp()
        {
            _offlineStore = new InMemoryStore();
        }

        protected override IPostCommentRepository CreateIPostCommentRepository()
        {
            return new InMemoryPostCommentRepository(_offlineStore);
        }
    }
}
