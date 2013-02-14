using System;
using NUnit.Framework;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public abstract class MessageProcessorFixtureBase<T> where T : MessageProcessorBase
    {
        protected const string c_key = "mailClientSecretKey";
        
        protected abstract string GetMessageCategory();

        protected abstract T CreateMessageProcessor();


        [Test]
        public void SupportedMessageCategoryTest()
        {
            var messageProcessor = CreateMessageProcessor();
            Assert.That(messageProcessor.SupportedMessageCategory, Is.EqualTo(GetMessageCategory()));
        }

        [Test]
        public void WrongMessageCategoryTest()
        {
            try
            {
                var subject = "FOO";
                var messageBody = "KEY: " + c_key + "\nTestbody";
                var parsedMessage = new ParsedMessage("me@here.com", "you@there.com", subject, messageBody, null);

                var messageProcessor = CreateMessageProcessor();
                messageProcessor.ProcessMessage(parsedMessage);

                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

        [Test]
        public void MissingMessageCategoryTest()
        {
            try
            {
                var subject = "";
                var messageBody = "KEY: " + c_key + "\nTestbody";
                var parsedMessage = new ParsedMessage("me@here.com", "you@there.com", subject, messageBody, null);

                var messageProcessor = CreateMessageProcessor();
                messageProcessor.ProcessMessage(parsedMessage);

                Assert.Fail();
            }
            catch (ArgumentException) { }
        }

    }
}
