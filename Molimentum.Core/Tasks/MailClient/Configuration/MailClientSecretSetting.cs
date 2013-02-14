using System.Configuration;

namespace Molimentum.Tasks.MailClient.Configuration
{
    public class MailClientSecretSetting : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
        }
    }
}