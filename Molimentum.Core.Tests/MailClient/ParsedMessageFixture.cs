using Molimentum.Tasks.MailClient.Configuration;
using Molimentum.Tasks.MailClient.PopMailProvider.Configuration;
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public class ParedMessageFixture
    {
        [Test]
        public void ParseMessageTest()
        {
            var from = "me@here.com";
            var to = "you@there.com";
            var subject = "testSubject";
            var body = "KEY1: VALUE1\nKEY2: VALUE2\n\nBody\nNONKEY3: NONVALUE3";
            var attachments = new MailAttachment[] { };

            var expectedCategory = subject;
            var expectedBody = "Body\r\nNONKEY3: NONVALUE3";
            var expectedValues = new Dictionary<string, string> { { "KEY1", "VALUE1" }, { "KEY2", "VALUE2" } };

            var parsedMessage = new ParsedMessage(from, to, subject, body, attachments);

            Assert.That(parsedMessage.From, Is.EqualTo(from));
            Assert.That(parsedMessage.To, Is.EqualTo(to));
            Assert.That(parsedMessage.Category, Is.EqualTo(expectedCategory));
            Assert.That(parsedMessage.Body, Is.EqualTo(expectedBody));
            Assert.That(parsedMessage, Is.EqualTo(expectedValues));
            Assert.That(parsedMessage["KEY1"], Is.EqualTo("VALUE1"));
            Assert.That(parsedMessage["KEY2"], Is.EqualTo("VALUE2"));
            Assert.That(parsedMessage["KEY3"], Is.Null);
        }

        [Test]
        public void ParseMessageTest2()
        {
            var from = "me@here.com";
            var to = "you@there.com";
            var subject = "testSubject";
            var body = "KEY1: VALUE1\nKEY2: VALUE2\nBody\nNONKEY3: NONVALUE3";
            var attachments = new MailAttachment[] { };

            var expectedCategory = subject;
            var expectedBody = "Body\r\nNONKEY3: NONVALUE3";
            var expectedValues = new Dictionary<string, string> { { "KEY1", "VALUE1" }, { "KEY2", "VALUE2" } };

            var parsedMessage = new ParsedMessage(from, to, subject, body, attachments);

            Assert.That(parsedMessage.From, Is.EqualTo(from));
            Assert.That(parsedMessage.To, Is.EqualTo(to));
            Assert.That(parsedMessage.Category, Is.EqualTo(expectedCategory));
            Assert.That(parsedMessage.Body, Is.EqualTo(expectedBody));
            Assert.That(parsedMessage, Is.EqualTo(expectedValues));
            Assert.That(parsedMessage["KEY1"], Is.EqualTo("VALUE1"));
            Assert.That(parsedMessage["KEY2"], Is.EqualTo("VALUE2"));
            Assert.That(parsedMessage["KEY3"], Is.Null);
        }
    }
}