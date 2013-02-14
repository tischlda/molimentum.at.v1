using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class YouTubeEmbeddingSetting : ConfigurationElement
    {
        [ConfigurationProperty("htmlTemplate", IsRequired = true)]
        public string HtmlTemplate
        {
            get
            {
                return (string)this["htmlTemplate"];
            }
        }
    }
}