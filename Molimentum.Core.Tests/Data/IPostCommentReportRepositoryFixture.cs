using System;
using System.Linq;
using System.Net.Mail;
using Molimentum.Repositories;
using NUnit.Framework;

namespace Molimentum.Tests
{
    [TestFixture]
    public abstract class IPostCommentRepositoryFixture
    {
        [Test]
        public void PostCommentNotFoundTest()
        {
            using (var repository = CreateIPostCommentRepository())
            {
                var postComment = repository.Get(Guid.Empty.ToString());

                Assert.IsNull(postComment);
            }
        }

        [Test]
        public void DeletePostCommentTest()
        {
            var author = "Author";
            var title = "Title";
            var website = new Uri("http://website.com");
            var email = new MailAddress("mail@mail.com");
            var body = "Body";

            using (var repository = CreateIPostCommentRepository())
            {
                var postCommentID = CreatePostComment(repository, author, title, website, email, body);

                var postComment = repository.Get(postCommentID);
                repository.Delete(postComment);
                repository.SubmitChanges();

                var postComment2 = repository.Get(postCommentID);

                Assert.IsNull(postComment2);
            }
        }

        [Test]
        public void LoadAndSavePostCommentTest()
        {
            var author = "Author";
            var title = "Title";
            var website = new Uri("http://website.com");
            var email = new MailAddress("mail@mail.com");
            var body = "Body";

            using (var repository = CreateIPostCommentRepository())
            {
                var postCommentID = CreatePostComment(repository, author, title, website, email, body);

                var postComment = repository.Get(postCommentID);

                Assert.AreEqual(author, postComment.Author);
                Assert.AreEqual(title, postComment.Title);
                Assert.AreEqual(website.ToString(), postComment.Website);
                Assert.AreEqual(email.Address, postComment.Email);
                Assert.AreEqual(body, postComment.Body);
            }
        }

        [Test] public void LoadAndSavePostCommentWithoutEmailAndWebsiteTest()
        {
            var author = "Author";
            var title = "Title";
            var website = (Uri)null;
            var email = (MailAddress) null;
            var body = "Body";

            using (var repository = CreateIPostCommentRepository())
            {
                var postCommentID = CreatePostComment(repository, author, title, website, email, body);

                var postComment = repository.Get(postCommentID);

                Assert.AreEqual(author, postComment.Author);
                Assert.AreEqual(title, postComment.Title);
                Assert.AreEqual(website, postComment.Website);
                Assert.AreEqual(email, postComment.Email);
                Assert.AreEqual(body, postComment.Body);
            }
        }

        [Test]
        public void ListPostCommentsTest()
        {
            var author = "Author";
            var title = "Title";
            var website = new Uri("http://website.com");
            var email = new MailAddress("mail@mail.com");
            var body = "Body";

            using (var repository = CreateIPostCommentRepository())
            {
                for (var i = 0; i < 20; i++)
                {
                    CreatePostComment(repository, author, title, website, email, body);
                }

                var postComments = repository.List(1, 20);

                Assert.That(postComments, Is.Not.Null);
                Assert.That(postComments.Items.Count(), Is.EqualTo(20));
            }
        }
        
        private static string CreatePostComment(IPostCommentRepository repository, string author, string title, Uri website, MailAddress email, string body)
        {
            var postComment = repository.Create();

            postComment.Author = author;
            postComment.Title = title;
            postComment.Website = website == null ? null : website.ToString();
            postComment.Body = body;
            postComment.Email = email == null ? null : email.Address;

            repository.Save(postComment);

            repository.SubmitChanges();

            return postComment.ID;
        }

        protected abstract IPostCommentRepository CreateIPostCommentRepository();
    }
}