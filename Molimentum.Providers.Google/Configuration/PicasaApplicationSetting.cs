using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class PicasaApplicationSetting : ConfigurationElement
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
