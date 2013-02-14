using Molimentum.Providers.Google.Configuration;
using NUnit.Framework;

namespace Molimentum.Providers.Google.Tests.Configuration
{
    // Expects the following settings in app.config:
    // <molimentum.providers.google.youTube>
    //   <application name="testYouTubeProviderApplicationName" />
    //   <authentication username="testYouTubeProviderUsername" password="testYouTubeProviderPassword" />
    //   <gallery user="testYouTubeProviderGalleryUser" />
    //   <embedding htmlTemplate="&lt;object foobar='asdf' /&gt;" />
    // </molimentum.providers.google.youTube>

    [TestFixture]
    public class YouTubeConfigurationFixture
    {
        [Test]
        public void CanLoadConfigurationTest()
        {
            var configuration = YouTubeConfiguration.Settings;

            Assert.That(configuration, Is.Not.Null);
        }

        [Test]
        public void SettingContainsDataTest()
        {
            var configuration = YouTubeConfiguration.Settings;

            Assert.That(configuration.Authentication.Username, Is.EqualTo("testYouTubeProviderUsername"));
            Assert.That(configuration.Authentication.Password, Is.EqualTo("testYouTubeProviderPassword"));
            Assert.That(configuration.Application.Name, Is.EqualTo("testYouTubeProviderApplicationName"));
            Assert.That(configuration.Gallery.User, Is.EqualTo("testYouTubeProviderGalleryUser"));
            Assert.That(configuration.Embedding.HtmlTemplate, Is.EqualTo("<object foobar='asdf' />"));
        }
    }
}
