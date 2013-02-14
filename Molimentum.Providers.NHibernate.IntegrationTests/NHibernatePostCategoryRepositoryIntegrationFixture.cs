using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tests;
using NHibernate.Cfg;
using NUnit.Framework;

namespace Molimentum.Providers.NHibernate.Tests
{
    [TestFixture]
    [Explicit("Integration")]
    [Category("Integration")]
    public class NHibernatePostCategoryRepositoryIntegrationFixture : IPostCategoryRepositoryFixture
    {
        private Configuration _configuration;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Post).Assembly);
        }

        protected override IPostCategoryRepository CreateIPostCategoryRepository()
        {
            return new NHibernatePostCategoryRepository(new NHibernateSessionBuilder());
        }
    }
}
