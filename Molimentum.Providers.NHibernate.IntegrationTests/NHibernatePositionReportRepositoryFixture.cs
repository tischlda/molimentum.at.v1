using Molimentum.Data.Repositories;
using Molimentum.Data.Tests;
using NUnit.Framework;

namespace Molimentum.Data.NHibernateProvider.Tests
{
    [TestFixture]
    public class NHibernatePositionReportRepositoryFixture : IPositionReportRepositoryFixture
    {
        protected override IPositionReportRepository CreateIPositionReportRepository()
        {
            return new NHibernatePositionReportRepository(new FakeDataContext(null, null));
        }
    }
}
