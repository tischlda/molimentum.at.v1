using System.Configuration;

namespace Molimentum.Tasks.MailClient.PopMailProvider.Configuration
{
    public class PopMailProviderConfiguration : ConfigurationSection
    {
        private static PopMailProviderConfiguration _mailClientConfiguration;

        public static PopMailProviderConfiguration Settings
        {
            get
            {
                if (_mailClientConfiguration == null)
                {
                    _mailClientConfiguration = (PopMailProviderConfiguration)ConfigurationManager.GetSection("molimentum.tasks.mailClient.popMailProvider");
                }

                return _mailClientConfiguration;
            }
        }

        [ConfigurationProperty("server", IsRequired = true)]
        public PopMailProviderServerSetting Server
        {
            get
            {
                return (PopMailProviderServerSetting) this["server"];
            }
        }
    }
}