using System;
using System.Linq;
using Molimentum.Model;
using Molimentum.Repositories;
using NUnit.Framework;

namespace Molimentum.Tests
{
    [TestFixture]
    public abstract class IPostRepositoryFixture
    {
        [Test]
        public void PostNotFoundTest()
        {
            using (var repository = CreateIPostRepository())
            {
                var post = repository.Get(Guid.Empty.ToString());

                Assert.IsNull(post);
            }
        }

        [Test]
        public void DeletePostTest()
        {
            var title = "Title 1";
            var body = "";
            var latitude = 42.24f;
            var longitude = 23.32f;
            var tags = new string[] {};
            var dateFrom = (DateTime?)null;
            var dateTo = (DateTime?)null;

            using (var repository = CreateIPostRepository())
            {
                var postID = CreatePost(repository, title, body, latitude, longitude, tags, true, dateFrom, dateTo);

                var post = repository.Get(postID);
                repository.Delete(post);
                repository.SubmitChanges();

                var post2 = repository.Get(postID);

                Assert.IsNull(post2);
            }
        }

        [Test]
        public void LoadAndSavePostTest()
        {
            var title = "Title 1";
            var body = "";
            var latitude = 42.24f;
            var longitude = 23.32f;
            var dateFrom = new DateTime(2010, 1, 1);
            var dateTo = new DateTime(2010, 1, 2);
            var tags = new[] { "Tag1", "Tag2" };

            string postID;

            using (var repository = CreateIPostRepository())
            {
                postID = CreatePost(repository, title, body, latitude, longitude, tags, true, dateFrom, dateTo);
            }
            
            using (var repository = CreateIPostRepository())
            {
                var post = repository.Get(postID);

                Assert.AreEqual(postID, post.ID);
                Assert.AreEqual(title, post.Title);
                Assert.AreEqual(body, post.Body);
                Assert.AreEqual(latitude, post.Position.Latitude);
                Assert.AreEqual(longitude, post.Position.Longitude);
                Assert.AreEqual(dateFrom, post.DateFrom);
                Assert.AreEqual(dateTo, post.DateTo);
                Assert.That(post.Tags.Count, Is.EqualTo(2));
                Assert.That(post.Tags, Has.Member(tags[0]));
                Assert.That(post.Tags, Has.Member(tags[1]));
            }
        }

        [Test]
        public void ListPostsTest()
        {
            var title = "Title 1";
            var body = "";
            var latitude = 42.24f;
            var longitude = 23.32f;
            var tags = new string[] { };
            var dateFrom = (DateTime?)null;
            var dateTo = (DateTime?)null;

            using (var repository = CreateIPostRepository())
            {
                for (var i = 0; i < 20; i++)
                {
                    CreatePost(repository, title + i, body, latitude, longitude, tags, i % 2 == 0, dateFrom, dateTo);
                }

                var posts = repository.List(1, 20);

                Assert.That(posts, Is.Not.Null);
                Assert.That(posts.Items.Count(), Is.EqualTo(20));
            }
        }
        
        [Test]
        public void ListPublishedPostsTest()
        {
            var title = "Title 1";
            var body = "";
            var latitude = 42.24f;
            var longitude = 23.32f;
            var tags = new string[] { };
            var dateFrom = (DateTime?)null;
            var dateTo = (DateTime?)null;

            using (var repository = CreateIPostRepository())
            {
                for (var i = 0; i < 20; i++)
                {
                    CreatePost(repository, title + i, body, latitude, longitude, tags, i % 2 == 0, dateFrom, dateTo);
                }

                var posts = repository.ListPublished(1, 20);

                Assert.That(posts, Is.Not.Null);
                Assert.That(posts.Items.Count(), Is.EqualTo(10));
                
                foreach(var post in posts.Items)
                {
                    Assert.That(post.IsPublished);
                }
            }
        }

        [Test]
        public void ListTagsTest()
        {
            var title = "Title 1";
            var body = "";
            var latitude = 42.24f;
            var longitude = 23.32f;
            var tags = new [] { "tag1", "tag2", "tag3" };
            var dateFrom = (DateTime?)null;
            var dateTo = (DateTime?)null;

            using (var repository = CreateIPostRepository())
            {
                for (var i = 0; i < 20; i++)
                {
                    CreatePost(repository, title + i, body, latitude, longitude, tags, i % 2 == 0, dateFrom, dateTo);
                }

                CreatePost(repository, title + "20", body, latitude, longitude, new[] { "tag1", "tag2", "tag4" }, false, dateFrom, dateTo);

                var tagSummaries = repository.ListTags();

                Assert.That(tagSummaries.Count(), Is.EqualTo(4));
                Assert.That(tagSummaries.Where(ts => ts.Tag == "tag1").First().Count, Is.EqualTo(21));
                Assert.That(tagSummaries.Where(ts => ts.Tag == "tag2").First().Count, Is.EqualTo(21));
                Assert.That(tagSummaries.Where(ts => ts.Tag == "tag3").First().Count, Is.EqualTo(20));
                Assert.That(tagSummaries.Where(ts => ts.Tag == "tag4").First().Count, Is.EqualTo(1));
            }
        }

        protected static string CreatePost(IPostRepository repository, string title, string body, float latitude, float longitude, string[] tags, bool isPublished, DateTime? dateFrom, DateTime? dateTo)
        {
            var post = repository.Create();

            post.Title = title;
            post.Body = body;
            post.Position = new Position(latitude, longitude);
            post.IsPublished = isPublished;
            post.DateFrom = dateFrom;
            post.DateTo = dateTo;
            
            foreach(var tag in tags) post.Tags.Add(tag);

            repository.Save(post);

            repository.SubmitChanges();

            return post.ID;
        }

        protected abstract IPostRepository CreateIPostRepository();
    }
}