using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class YouTubeApplicationSetting : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }
    }
}