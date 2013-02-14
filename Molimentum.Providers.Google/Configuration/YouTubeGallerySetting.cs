using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class YouTubeGallerySetting : ConfigurationElement
    {
        [ConfigurationProperty("user", IsRequired = true)]
        public string User
        {
            get
            {
                return (string)this["user"];
            }
        }
    }
}