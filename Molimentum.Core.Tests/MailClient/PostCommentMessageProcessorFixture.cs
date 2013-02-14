using System;
using Molimentum.Model;
using Molimentum.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public class PostCommentMessageProcessorFixture : MessageProcessorFixtureBase<PostCommentMessageProcessor>
    {
        protected override string GetMessageCategory()
        {
            return "POSTCOMMENT";
        }

        protected override PostCommentMessageProcessor CreateMessageProcessor()
        {
            return new PostCommentMessageProcessor(null, null);
        }

        [Test]
        public void ProcessPostCommentMessageTest()
        {
            var subject = "POSTCOMMENT";
            var messageBody = "KEY: " + c_key + "\nTITLE: Testtitle\nAUTHOR: Testauthor\nEMAIL: test@test.com\nWEBSITE: http://www.test.com\nPOSTID: 00E57B36-70D9-4181-B662-9DB0422EF314\nTestbody";

            var expectedTitle = "Testtitle";
            var expectedBody = "Testbody";
            var expectedAuthor = "Testauthor";
            var expectedEmail = "test@test.com";
            var expectedWebsite = "http://www.test.com";
            var expectedPostId = "00E57B36-70D9-4181-B662-9DB0422EF314";

            var postComment = ProcessPostCommentMessageTest(subject, messageBody, expectedTitle, expectedBody, expectedAuthor, expectedPostId, expectedEmail, expectedWebsite);
        }

        [Test]
        public void ProcessPostCommentMessageWithoutPostTest()
        {
            var subject = "POSTCOMMENT";
            var messageBody = "KEY: " + c_key + "\nTITLE: Testtitle\nAUTHOR: Testauthor\nTestbody";

            var expectedTitle = "Testtitle";
            var expectedBody = "Testbody";
            var expectedAuthor = "Testauthor";
            var expectedEmail = (string) null;
            var expectedWebsite= (string)null;
            var expectedPostId = (string)null;

            var postComment = ProcessPostCommentMessageTest(subject, messageBody, expectedTitle, expectedBody, expectedAuthor, expectedPostId, expectedEmail, expectedWebsite);
        }

        private static PostComment ProcessPostCommentMessageTest(string subject, string messageBody, string expectedTitle, string expectedBody, string expectedAuthor, string expectedPostId, string expectedEmail, string expectedWebsite)
        {
            var postComment = ProcessMessage(subject, messageBody);

            Assert.That(postComment.Title, Is.EqualTo(expectedTitle));
            Assert.That(postComment.Body, Is.EqualTo(expectedBody));
            Assert.That(postComment.Author, Is.EqualTo(expectedAuthor));
            if(expectedPostId != null)
            {
                Assert.That(postComment.Post.ID, Is.EqualTo(expectedPostId));
            }
            else
            {
                Assert.That(postComment.Post, Is.Null);
            }
            Assert.That(postComment.Email, Is.EqualTo(expectedEmail));
            Assert.That(postComment.Website, Is.EqualTo(expectedWebsite));

            return postComment;
        }

        private static PostComment ProcessMessage(string subject, string messageBody)
        {
            var postComment = new PostComment();

            var postCommentRepositoryMock = MockRepository.GenerateMock<IPostCommentRepository>();
            postCommentRepositoryMock.Expect(b => b.Create()).Return(postComment);
            postCommentRepositoryMock.Expect(b => b.SubmitChanges());

            var post = new Post();
            post.ID = "00E57B36-70D9-4181-B662-9DB0422EF314";
            var postRepositoryMock = MockRepository.GenerateMock<IPostRepository>();
            postRepositoryMock.Expect(b => b.Get(post.ID)).Return(post);

            var parsedMessage = new ParsedMessage("me@here.com", "postComment@molimentum.at", subject, messageBody, null);

            var postCommentMessageProcessor = new PostCommentMessageProcessor(postCommentRepositoryMock, postRepositoryMock);
            postCommentMessageProcessor.ProcessMessage(parsedMessage);

            postCommentRepositoryMock.VerifyAllExpectations();
            return postComment;
        }
    }
}
