using Molimentum.Data.Repositories;
using Molimentum.Data.Tests;
using NUnit.Framework;

namespace Molimentum.Data.NHibernateProvider.Tests
{
    [TestFixture]
    public class NHibernateBlogRepositoryFixture : IBlogRepositoryFixture
    {
        private readonly IMolimentumDataContext _fakeDataContext = new FakeDataContext(null, null);

        protected override IBlogRepository CreateIBlogRepository()
        {
            return new NHibernateBlogRepository(_fakeDataContext);
        }
    }
}
