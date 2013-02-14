using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class PicasaGallerySetting : ConfigurationElement
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
