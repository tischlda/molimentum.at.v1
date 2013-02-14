using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tests;
using NHibernate.Cfg;
using NUnit.Framework;
using System;

namespace Molimentum.Providers.NHibernate.Tests
{
    [TestFixture]
    [Explicit("Integration")]
    [Category("Integration")]
    public class NHibernatePostRepositoryIntegrationFixture : IPostRepositoryFixture
    {
        private Configuration _configuration;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Post).Assembly);
        }
        
        protected override IPostRepository CreateIPostRepository()
        {
            return new NHibernatePostRepository(new NHibernateSessionBuilder());
        }

        [Test]
        public void LoadAndSavePostWithLongTextTest()
        {
            var title = "Title 1";
            var body = new String('x', 4001);
            var latitude = 42.24f;
            var longitude = 23.32f;
            var tags = new[] { "Tag1", "Tag2" };

            string postID;

            using (var repository = CreateIPostRepository())
            {
                postID = CreatePost(repository, title, body, latitude, longitude, tags, true, null, null);
            }

            using (var repository = CreateIPostRepository())
            {
                var post = repository.Get(postID);

                Assert.AreEqual(postID, post.ID);
                Assert.AreEqual(title, post.Title);
                Assert.AreEqual(body, post.Body);
                Assert.AreEqual(latitude, post.Position.Latitude);
                Assert.AreEqual(longitude, post.Position.Longitude);
                Assert.That(post.Tags.Count, Is.EqualTo(2));
                Assert.That(post.Tags, Has.Member(tags[0]));
                Assert.That(post.Tags, Has.Member(tags[1]));
            }
        }
    }
}
