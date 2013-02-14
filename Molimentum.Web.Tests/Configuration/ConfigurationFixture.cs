using System.Linq;
using Molimentum.Web.Configuration;
using NUnit.Framework;

namespace Molimentum.Web.Tests.Controllers
{
    // Assumes the following settings in app.config:
    // <molimentum.web>
    //   <widgets>
    //     <widget name="LatestPositions" />
    //     <widget name="LatestAlbums" adminOnly="false" />
    //     <widget name="Administration" adminOnly="true" />
    //   <specialPosts>
    //     <specialPost id="40B3BE69-A3FF-4246-A549-DD68B59735AB" alternativeUrl="Impressum" />
    //  </specialPosts>
    // </molimentum.web>

    [TestFixture]
    public class ConfigurationFixture
    {
        [Test]
        public void CanLoadConfigurationTest()
        {
            var configuration = WebConfiguration.Settings;

            Assert.That(configuration, Is.Not.Null);
        }

        [Test]
        public void WidgetSettingsNotEmptyTest()
        {
            var configuration = WebConfiguration.Settings;

            Assert.That(configuration.Widgets, Is.Not.Empty);
        }

        [Test]
        public void WidgetSettingContainsDataTest()
        {
            var configuration = WebConfiguration.Settings;

            Assert.That(configuration.Widgets.Settings.Count(), Is.EqualTo(3));

            Assert.That(configuration.Widgets.Settings.ElementAt(0).Name, Is.EqualTo("LatestPositions"));
            Assert.That(configuration.Widgets.Settings.ElementAt(0).AdminOnly, Is.False);
            
            Assert.That(configuration.Widgets.Settings.ElementAt(1).Name, Is.EqualTo("LatestAlbums"));
            Assert.That(configuration.Widgets.Settings.ElementAt(1).AdminOnly, Is.False);

            Assert.That(configuration.Widgets.Settings.ElementAt(2).Name, Is.EqualTo("Administration"));
            Assert.That(configuration.Widgets.Settings.ElementAt(2).AdminOnly, Is.True);
        }

        [Test]
        public void SpecialPostSettingsNotEmptyTest()
        {
            var configuration = WebConfiguration.Settings;

            Assert.That(configuration.SpecialPosts, Is.Not.Empty);
        }

        [Test]
        public void SpecialPostSettingContainsDataTest()
        {
            var configuration = WebConfiguration.Settings;
            var specialPost = configuration.SpecialPosts.Settings.First();

            Assert.That(specialPost.Id, Is.EqualTo("40B3BE69-A3FF-4246-A549-DD68B59735AB"));
            Assert.That(specialPost.AlternativeUrl, Is.EqualTo("Impressum"));
        }
    }
}