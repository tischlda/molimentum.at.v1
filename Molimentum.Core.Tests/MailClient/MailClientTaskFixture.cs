using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public class MailClientTaskFixture
    {
        private const string c_key = "mailClientSecretKey";

        [Test]
        public void ProecessMailTest()
        {
            var mockRepository = new MockRepository();

            var message = new MailMessage("me@here.com", "you@there.com", "TESTCATEGORY", "KEY: mailClientSecretKey\nKEY1: VALUE1\nKEY2: VALUE2\n\nTESTBODY", null);
            var expectedCategory = "TESTCATEGORY";
            var expectedFrom = "me@here.com";
            var expectedTo = "you@there.com";
            var expectedBody = "TESTBODY";
            var expectedAttributes = new Dictionary<string, string> { {"KEY", "mailClientSecretKey"}, {"KEY1", "VALUE1"}, {"KEY2", "VALUE2"} };

            var mailClientMock = mockRepository.StrictMock<IMailService>();
            mailClientMock.Expect(m => m.Connect());
            mailClientMock.Expect(m => m.HasMessages).Return(true);
            mailClientMock.Expect(m => m.FetchNext()).Return(message);
            mailClientMock.Expect(m => m.HasMessages).Return(false);
            mailClientMock.Expect(m => m.Disconnect());

            var messageProcessorMock = mockRepository.StrictMock<IMessageProcessor>();
            messageProcessorMock.Expect(m => m.SupportedMessageCategory).Return("TESTCATEGORY");
            messageProcessorMock.Expect(m => m.ProcessMessage(null)).IgnoreArguments()
                .WhenCalled(m => VerifyParsedMessage((ParsedMessage)m.Arguments[0], expectedCategory, expectedFrom, expectedTo, expectedBody, expectedAttributes));
            
            mockRepository.ReplayAll();

            var mailClientTask = new MailClientTask(mailClientMock, new [] { messageProcessorMock });
            mailClientTask.Execute();

            mockRepository.VerifyAll();
        }

        [Test]
        public void ProecessMultipleMailsTest()
        {
            var mockRepository = new MockRepository();

            var message1 = new MailMessage("me1@here.com", "you1@there.com", "TESTCATEGORY1", "KEY: mailClientSecretKey\nKEY1: VALUE1\nKEY2: VALUE2\n\nTESTBODY1", null);
            var expectedCategory1 = "TESTCATEGORY1";
            var expectedFrom1 = "me1@here.com";
            var expectedTo1 = "you1@there.com";
            var expectedBody1 = "TESTBODY1";
            var expectedAttributes1 = new Dictionary<string, string> { { "KEY", "mailClientSecretKey" }, { "KEY1", "VALUE1" }, { "KEY2", "VALUE2" } };

            var message2 = new MailMessage("me2@here.com", "you2@there.com", "TESTCATEGORY2", "KEY: mailClientSecretKey\nKEY3: VALUE3\nKEY4: VALUE4\n\nTESTBODY2", null);
            var expectedCategory2 = "TESTCATEGORY2";
            var expectedFrom2 = "me2@here.com";
            var expectedTo2 = "you2@there.com";
            var expectedBody2 = "TESTBODY2";
            var expectedAttributes2 = new Dictionary<string, string> { { "KEY", "mailClientSecretKey" }, { "KEY3", "VALUE3" }, { "KEY4", "VALUE4" } };

            var mailClientMock = mockRepository.StrictMock<IMailService>();
            mailClientMock.Expect(m => m.Connect());
            mailClientMock.Expect(m => m.HasMessages).Return(true);
            mailClientMock.Expect(m => m.FetchNext()).Return(message1);
            mailClientMock.Expect(m => m.HasMessages).Return(true);
            mailClientMock.Expect(m => m.FetchNext()).Return(message2);
            mailClientMock.Expect(m => m.HasMessages).Return(false);
            mailClientMock.Expect(m => m.Disconnect());

            var messageProcessorMock1 = mockRepository.StrictMock<IMessageProcessor>();
            messageProcessorMock1.Expect(m => m.SupportedMessageCategory).Return("TESTCATEGORY1");
            messageProcessorMock1.Expect(m => m.ProcessMessage(null)).IgnoreArguments()
                .WhenCalled(m => VerifyParsedMessage((ParsedMessage)m.Arguments[0], expectedCategory1, expectedFrom1, expectedTo1, expectedBody1, expectedAttributes1));

            var messageProcessorMock2 = mockRepository.StrictMock<IMessageProcessor>();
            messageProcessorMock2.Expect(m => m.SupportedMessageCategory).Return("TESTCATEGORY2");
            messageProcessorMock2.Expect(m => m.ProcessMessage(null)).IgnoreArguments()
                .WhenCalled(m => VerifyParsedMessage((ParsedMessage)m.Arguments[0], expectedCategory2, expectedFrom2, expectedTo2, expectedBody2, expectedAttributes2));

            mockRepository.ReplayAll();

            var mailClientTask = new MailClientTask(mailClientMock, new[] { messageProcessorMock1, messageProcessorMock2 });
            mailClientTask.Execute();

            mockRepository.VerifyAll();
        }

        [Test]
        public void IgnoreSpamMailTest()
        {
            var mockRepository = new MockRepository();

            var message = new MailMessage("me@here.com", "you@there.com", string.Format("V1@GR4"), "Body", null);

            var mailClientMock = mockRepository.StrictMock<IMailService>();
            mailClientMock.Expect(m => m.Connect());
            mailClientMock.Expect(m => m.HasMessages).Return(true);
            mailClientMock.Expect(m => m.FetchNext()).Return(message);
            mailClientMock.Expect(m => m.HasMessages).Return(false);
            mailClientMock.Expect(m => m.Disconnect());

            var messageProcessorMock = mockRepository.StrictMock<IMessageProcessor>();
            messageProcessorMock.Expect(m => m.SupportedMessageCategory).Return("TESTCATEGORY");

            mockRepository.ReplayAll();

            var mailClientTask = new MailClientTask(mailClientMock, new[] { messageProcessorMock });
            mailClientTask.Execute();

            mockRepository.VerifyAll();
        }

        private void VerifyParsedMessage(ParsedMessage parsedMessage, string expectedCategory, string expectedFrom,
            string expectedTo, string expectedBody, IEnumerable<KeyValuePair<string, string>> expectedAttributes)
        {
            Assert.That(parsedMessage.Category, Is.EqualTo(expectedCategory));
            Assert.That(parsedMessage.From, Is.EqualTo(expectedFrom));
            Assert.That(parsedMessage.To, Is.EqualTo(expectedTo));
            Assert.That(parsedMessage.Body, Is.EqualTo(expectedBody));
            Assert.That(parsedMessage, Is.EqualTo(expectedAttributes));
        }

    }
}
