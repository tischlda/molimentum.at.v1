using System.Configuration;

namespace Molimentum.Web.Configuration
{
    public class SpecialPostSetting : ConfigurationElement
    {
        [ConfigurationProperty("alternativeUrl", IsRequired = true, IsKey = true)]
        public string AlternativeUrl
        {
            get
            {
                return (string)this["alternativeUrl"];
            }
        }

        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }
        }
    }
}
