using System;
using NUnit.Framework;
using Molimentum.Model;

namespace Molimentum.Tests
{
    [TestFixture]
    public class PostCommentFixture
    {
        [Test]
        public void ToStringWithoutPostTest()
        {
            var publishDate = new DateTime(2010, 01, 01);
            
            var postComment = new PostComment
            {
                Author = "author",
                PublishDate = publishDate,
                Title = "title",
                Email = "email",
                Website = "website",
                Body = "body"
            };

            var expectedString =
                "Post:  " + Environment.NewLine +
                "Author: author" + Environment.NewLine +
                "PublishDate: " + publishDate.ToString() + "" + Environment.NewLine +
                "Title: title" + Environment.NewLine +
                "Email: email" + Environment.NewLine +
                "Website: website" + Environment.NewLine +
                Environment.NewLine +
                "body";

            var result = postComment.ToString();

            Assert.That(result, Is.EqualTo(expectedString));
        }

        [Test]
        public void ToStringWithPostTest()
        {
            var publishDate = new DateTime(2010, 01, 01);
            var postID = "TestPostID";

            var post = new Post
            {
                ID = postID,
                Title = "postTitle"
            };

            var postComment = new PostComment
            {
                Post = post,
                Author = "author",
                PublishDate = publishDate,
                Title = "title",
                Email = "email",
                Website = "website",
                Body = "body"
            };

            var expectedString =
                "Post: TestPostID postTitle" + Environment.NewLine +
                "Author: author" + Environment.NewLine +
                "PublishDate: " + publishDate.ToString() + "" + Environment.NewLine +
                "Title: title" + Environment.NewLine +
                "Email: email" + Environment.NewLine +
                "Website: website" + Environment.NewLine +
                Environment.NewLine +
                "body";

            var result = postComment.ToString();

            Assert.That(result, Is.EqualTo(expectedString));
        }
    }
}
