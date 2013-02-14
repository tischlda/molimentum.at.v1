using Molimentum.Tasks.MailClient.Configuration;
using Molimentum.Tasks.MailClient.PopMailProvider.Configuration;
using NUnit.Framework;

namespace Molimentum.Tasks.MailClient.Tests
{
    [TestFixture]
    public class ConfigurationFixture
    {
        [Test]
        public void MailClientConfigurationTest()
        {
            Assert.That(MailClientConfiguration.Settings, Is.Not.Null);
            Assert.That(MailClientConfiguration.Settings.Secret.Key, Is.EqualTo("mailClientSecretKey"));
            Assert.That(MailClientConfiguration.Settings.Pictures.AlbumID, Is.EqualTo("mailClientPicturesAlbumID"));
        }

        [Test]
        public void PopMailProviderConfigurationTest()
        {
            Assert.That(PopMailProviderConfiguration.Settings, Is.Not.Null);
            Assert.That(PopMailProviderConfiguration.Settings.Server.Name, Is.EqualTo("testPopMailProviderServername"));
            Assert.That(PopMailProviderConfiguration.Settings.Server.Port, Is.EqualTo(110));
            Assert.That(PopMailProviderConfiguration.Settings.Server.Username, Is.EqualTo("testPopMailProviderUsername"));
            Assert.That(PopMailProviderConfiguration.Settings.Server.Password, Is.EqualTo("testPopMailProviderPassword"));
        }
    }
}