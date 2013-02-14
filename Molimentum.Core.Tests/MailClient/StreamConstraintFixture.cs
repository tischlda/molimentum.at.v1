using Molimentum.Tasks.MailClient.Configuration;
using Molimentum.Tasks.MailClient.PopMailProvider.Configuration;
using NUnit.Framework;
using System.IO;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public class StreamConstraintFixture
    {
        [Test]
        public void EqualStreamDataTest()
        {
            var data = new byte[] { 1, 2, 4 };
            var streamData = new byte[] { 1, 2, 4 };

            var stream = new MemoryStream(streamData);

            var streamConstraint = new StreamConstraint(data);

            Assert.That(streamConstraint.Eval(stream), Is.True);
        }

        [Test]
        public void UnequalStreamDataTest()
        {
            var data = new byte[] { 1, 2, 4 };
            var streamData = new byte[] { 2, 4, 8 };

            var stream = new MemoryStream(streamData);

            var streamConstraint = new StreamConstraint(data);

            Assert.That(streamConstraint.Eval(stream), Is.False);
        }

        [Test]
        public void OtherObjectTest()
        {
            var data = new byte[] { 1, 2, 4 };

            var o = new object();

            var streamConstraint = new StreamConstraint(data);

            Assert.That(streamConstraint.Eval(o), Is.False);
        }
    }
}