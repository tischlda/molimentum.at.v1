using System;
using Molimentum.Model;
using Molimentum.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;
using System.IO;
using RM = Rhino.Mocks.Constraints;
using Molimentum;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public class PostMessageProcessorFixture : MessageProcessorFixtureBase<PostMessageProcessor>
    {
        protected override string GetMessageCategory()
        {
            return "POST";
        }

        protected override PostMessageProcessor CreateMessageProcessor()
        {
            return new PostMessageProcessor(null, null, null);
        }

        [Test]
        public void ProcessPostMessageTest1()
        {
            var subject = "POST";
            var messageBody = "KEY: " + c_key + "\nTITLE: Testtitle\nCATEGORY: Reise\nLATITUDE: 01°00.00'N\nLONGITUDE: 002°00.00'E\nTIME: 1.2.2009 00:00\nDATEFROM: 2.2.2009\nDATETO: 3.2.2009\nTAGS: foo;bar\nTestbody";

            var expectedTitle = "Testtitle";
            var expectedBody = "Testbody";
            var expectedPosition = new Position(1, 2);
            var expectedPublishDate = new DateTime(2009, 2, 1, 0, 0, 0);
            var expectedPositionDateTime = new DateTime(2009, 2, 1, 0, 0, 0);
            var expectedDateFrom = new DateTime(2009, 2, 2, 0, 0, 0);
            var expectedDateTo = new DateTime(2009, 2, 3, 0, 0, 0);
            var expectedCategory = "Reise";
            var expectedTags = new[] { "foo", "bar" };

            var post = ProcessPostMessageTest(subject, messageBody, null, expectedTitle, expectedBody, expectedPositionDateTime, expectedPosition, expectedCategory, expectedTags, expectedDateFrom, expectedDateTo);

            Assert.That(post.PublishDate, Is.EqualTo(expectedPublishDate));
            Assert.That(post.PositionDateTime, Is.EqualTo(expectedPositionDateTime));
        }

        [Test]
        public void ProcessPostMessageTest2()
        {
            var subject = "POST";
            var messageBody = "KEY: " + c_key + "\nCATEGORY: Reise\nTITLE: Testtitle\n\nTestbody";

            var expectedTitle = "Testtitle";
            var expectedBody = "Testbody";
            var expectedPositionDateTime = (DateTime?)null;
            var expectedDateFrom = (DateTime?)null;
            var expectedDateTo = (DateTime?)null;
            var expectedPosition = (Position)null;
            var expectedCategory = "Reise";
            var expectedTags = new string[] { };

            var post = ProcessPostMessageTest(subject, messageBody, null, expectedTitle, expectedBody, expectedPositionDateTime, expectedPosition, expectedCategory, expectedTags, expectedDateFrom, expectedDateTo);

            Assert.That((DateTime.Now - post.PublishDate).Value, Is.LessThan(TimeSpan.FromMinutes(1)));
            Assert.That((DateTime.Now - post.PositionDateTime).Value, Is.LessThan(TimeSpan.FromMinutes(1)));
        }

        [Test]
        [Ignore("Ignored temporary.")]
        public void ProcessPostMessageTest3()
        {
            var subject = "POST";
            var messageBody = "KEY: " + c_key + "\nTITLE: Testtitle\nCATEGORY: Reise\nLATITUDE: 01°00.00'N\nLONGITUDE: 002°00.00'E\nTIME: 1.2.2009 00:00\nTAGS: foo;bar\nTestbody\n[PICTURE1]\nFoo\n[PICTURE3]\nBar\n[PICTURE2]";
            var attachments = new [] {
                new MailAttachment("testPicture1", new byte[] { 1, 2, 4 } ),
                new MailAttachment("testPicture2", new byte[] { 2, 4, 8 } ),
                new MailAttachment("testPicture3", new byte[] { 4, 8, 16 } )
            };

            var expectedTitle = "Testtitle";
            var expectedBody =
                "Testbody\r\n" +
                "<a href='http://testpicture1/' rel='lightbox' title='testPicture1'><img src='http://thumbnail.testpicture1/' alt='testPicture1' /></a>\r\n" +
                "Foo\r\n" +
                "<a href='http://testpicture3/' rel='lightbox' title='testPicture3'><img src='http://thumbnail.testpicture3/' alt='testPicture3' /></a>\r\n" +
                "Bar\r\n" +
                "<a href='http://testpicture2/' rel='lightbox' title='testPicture2'><img src='http://thumbnail.testpicture2/' alt='testPicture2' /></a>";
            var expectedPosition = new Position(1, 2);
            var expectedPublishDate = new DateTime(2009, 2, 1, 0, 0, 0);
            var expectedPositionDateTime = new DateTime(2009, 2, 1, 0, 0, 0);
            var expectedDateFrom = (DateTime?)null;
            var expectedDateTo = (DateTime?)null;
            var expectedCategory = "Reise";
            var expectedTags = new[] { "foo", "bar" };

            var post = ProcessPostMessageTest(subject, messageBody, attachments, expectedTitle, expectedBody, expectedPositionDateTime, expectedPosition, expectedCategory, expectedTags, expectedDateFrom, expectedDateTo);

            Assert.That(post.PublishDate, Is.EqualTo(expectedPublishDate));
            Assert.That(post.PositionDateTime, Is.EqualTo(expectedPositionDateTime));
        }

        private static Post ProcessPostMessageTest(string subject, string messageBody, IEnumerable<MailAttachment> attachments, string expectedTitle, string expectedBody, DateTime? expectedPositionDateTime, Position expectedPosition, string expectedCategory, string[] expectedTags, DateTime? expectedDateFrom, DateTime? expectedDateTo)
        {
            var post = ProcessMessage(subject, messageBody, expectedCategory, attachments, expectedPositionDateTime, expectedPosition);

            Assert.That(post.Title, Is.EqualTo(expectedTitle));
            Assert.That(post.Body, Is.EqualTo(expectedBody));
            Assert.That(post.IsPublished, Is.EqualTo(true));
            Assert.That(post.Position, Is.EqualTo(expectedPosition));
            Assert.That(post.Tags, Is.EqualTo(expectedTags));
            Assert.That(post.DateFrom, Is.EqualTo(expectedDateFrom));
            Assert.That(post.DateTo, Is.EqualTo(expectedDateTo));

            return post;
        }

        private static Post ProcessMessage(string subject, string messageBody, string expectedCategory, IEnumerable<MailAttachment> attachments, DateTime? expectedPositionDateTime, Position expectedPosition)
        {
            var post = new Post();

            var postRepositoryMock = MockRepository.GenerateMock<IPostRepository>();
            postRepositoryMock.Expect(b => b.Create()).Return(post);
            postRepositoryMock.Expect(b => b.SubmitChanges());

            var postCategory = new PostCategory();

            var postCategoryRepositoryMock = MockRepository.GenerateMock<IPostCategoryRepository>();
            postCategoryRepositoryMock.Expect(b => b.GetByTitle(expectedCategory)).Return(postCategory);


            var pictureRepositoryMock = MockRepository.GenerateMock<IPictureRepository>();
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    pictureRepositoryMock.Expect(m => m.AddPictureToAlbum("mailClientPicturesAlbumID", null, attachment.Name, "", "", expectedPositionDateTime, expectedPosition))
                        .Constraints(new[] { RM.Is.Equal("mailClientPicturesAlbumID"), new StreamConstraint(attachment.Data), RM.Is.Equal(attachment.Name), RM.Is.Equal(""), RM.Is.Equal(""), RM.Is.Equal(expectedPositionDateTime), RM.Is.Equal(expectedPosition) })
                        .Return(
                            new Picture
                            {
                                PictureUri = new Uri("http://" + attachment.Name),
                                ThumbnailUri = new Uri("http://thumbnail." + attachment.Name),
                                Title = attachment.Name
                            });
                }
            }

            var parsedMessage = new ParsedMessage("me@here.com", "you@there.com", subject, messageBody, attachments);

            var postMessageProcessor = new PostMessageProcessor(postRepositoryMock, postCategoryRepositoryMock, pictureRepositoryMock);
            postMessageProcessor.ProcessMessage(parsedMessage);

            postRepositoryMock.VerifyAllExpectations();
            postCategoryRepositoryMock.VerifyAllExpectations();
            pictureRepositoryMock.VerifyAllExpectations();

            return post;
        }
    }
}
