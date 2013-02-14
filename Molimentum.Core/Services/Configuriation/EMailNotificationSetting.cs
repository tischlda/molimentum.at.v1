using System.Configuration;

namespace Molimentum.Services.Configuration
{
    public class EMailNotificationSetting : ConfigurationElement
    {
        [ConfigurationProperty("from", IsRequired = true)]
        public string From
        {
            get
            {
                return (string)this["from"];
            }
        }

        [ConfigurationProperty("to", IsRequired = true)]
        public string To
        {
            get
            {
                return (string)this["to"];
            }
        }
    }
}
