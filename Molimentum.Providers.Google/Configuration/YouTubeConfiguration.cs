using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class YouTubeConfiguration : ConfigurationSection
    {
        private static YouTubeConfiguration _youTubeConfiguration;

        public static YouTubeConfiguration Settings
        {
            get
            {
                if (_youTubeConfiguration == null)
                {
                    _youTubeConfiguration = (YouTubeConfiguration)ConfigurationManager.GetSection("molimentum.providers.google.youTube");
                }

                return _youTubeConfiguration;
            }
        }

        [ConfigurationProperty("authentication", IsRequired = true)]
        public YouTubeAuthenticationSetting Authentication
        {
            get
            {
                return (YouTubeAuthenticationSetting) this["authentication"];
            }
        }

        [ConfigurationProperty("application", IsRequired = true)]
        public YouTubeApplicationSetting Application
        {
            get
            {
                return (YouTubeApplicationSetting)this["application"];
            }
        }

        [ConfigurationProperty("gallery", IsRequired = true)]
        public YouTubeGallerySetting Gallery
        {
            get
            {
                return (YouTubeGallerySetting)this["gallery"];
            }
        }

        [ConfigurationProperty("embedding", IsRequired = true)]
        public YouTubeEmbeddingSetting Embedding
        {
            get
            {
                return (YouTubeEmbeddingSetting)this["embedding"];
            }
        }
    }
}