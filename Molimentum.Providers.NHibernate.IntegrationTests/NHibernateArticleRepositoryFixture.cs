using Molimentum.Data.Repositories;
using Molimentum.Data.Tests;
using NUnit.Framework;

namespace Molimentum.Data.NHibernateProvider.Tests
{
    [TestFixture]
    public class NHibernateArticleRepositoryFixture : IArticleRepositoryFixture
    {
        protected override IArticleRepository CreateIArticleRepository()
        {
            return new NHibernateArticleRepository(new FakeDataContext(null, null));
        }
    }
}
