using System;
using Molimentum.Model;
using Molimentum.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public class PictureMessageProcessorFixture : MessageProcessorFixtureBase<PictureMessageProcessor>
    {
        protected override string GetMessageCategory()
        {
            return "PICTURE";
        }

        protected override PictureMessageProcessor CreateMessageProcessor()
        {
            return new PictureMessageProcessor(null);
        }

        [Test]
        public void ProcessPictureMessageTest()
        {
            var subject = "PICTURE";
            var messageBody = "KEY: " + c_key + "\nTITLE: Testtitle\nTestbody";

            var expectedTitle = "Testtitle";
            var expectedBody = "Testbody";
            var image = new byte[] { 0, 1, 0 };

            var picture = ProcessPictureMessageTest(subject, messageBody, expectedTitle, expectedBody, image);
        }

        private static Picture ProcessPictureMessageTest(string subject, string messageBody, string expectedTitle, string expectedBody, byte[] image)
        {
            var picture = new Picture();

            var pictureRepositoryMock = MockRepository.GenerateMock<IPictureRepository>();
            pictureRepositoryMock.Expect(b => b.AddPictureToAlbum(null, null, null, null, null, null, null)).IgnoreArguments().Return(picture);

            var mailAttachment = new MailAttachment("image", image);

            var parsedMessage = new ParsedMessage("me@here.com", "picture@molimentum.at", subject, messageBody, new[] { mailAttachment });

            var pictureMessageProcessor = new PictureMessageProcessor(pictureRepositoryMock);
            pictureMessageProcessor.ProcessMessage(parsedMessage);

            pictureRepositoryMock.VerifyAllExpectations();
            return picture;
        }
    }
}
