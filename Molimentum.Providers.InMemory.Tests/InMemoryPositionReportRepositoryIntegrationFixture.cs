using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.InMemory.Tests
{
    public class InMemoryPositionReportRepositoryIntegrationFixture : IPositionReportRepositoryFixture
    {
        private InMemoryStore _offlineStore;

        [SetUp]
        public void SetUp()
        {
            _offlineStore = new InMemoryStore();
        }

        protected override IPositionReportRepository CreateIPositionReportRepository()
        {
            return new InMemoryPositionReportRepository(_offlineStore);
        }
    }
}
