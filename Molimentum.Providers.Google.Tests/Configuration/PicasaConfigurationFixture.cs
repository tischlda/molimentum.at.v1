using Molimentum.Providers.Google.Configuration;
using NUnit.Framework;

namespace Molimentum.Providers.Google.Tests.Configuration
{
    // Expects the following settings in app.config:
    // <molimentum.providers.google.picasa>
    //   <application name="testPicasaProviderApplicationName" />
    //   <authentication username="testPicasaProviderUsername" password="testPicasaProviderPassword" />
    //   <gallery user="testPicasaProviderGalleryUser" />
    //   <pictures thumbnailSize="144" maximumImageSize="640" />
    // </molimentum.providers.google.picasa>

    [TestFixture]
    public class PicasaConfigurationFixture
    {
        [Test]
        public void CanLoadConfigurationTest()
        {
            var configuration = PicasaConfiguration.Settings;

            Assert.That(configuration, Is.Not.Null);
        }

        [Test]
        public void SettingContainsDataTest()
        {
            var configuration = PicasaConfiguration.Settings;

            Assert.That(configuration.Authentication.Username, Is.EqualTo("testPicasaProviderUsername"));
            Assert.That(configuration.Authentication.Password, Is.EqualTo("testPicasaProviderPassword"));
            Assert.That(configuration.Application.Name, Is.EqualTo("testPicasaProviderApplicationName"));
            Assert.That(configuration.Gallery.User, Is.EqualTo("testPicasaProviderGalleryUser"));
            Assert.That(configuration.Pictures.ThumbnailSize, Is.EqualTo(144));
            Assert.That(configuration.Pictures.MaximumImageSize, Is.EqualTo(640));
        }
    }
}
