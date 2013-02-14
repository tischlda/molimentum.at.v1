using System.Configuration;

namespace Molimentum.Tasks.MailClient.Configuration
{
    public class MailClientConfiguration : ConfigurationSection
    {
        private static MailClientConfiguration _mailClientConfiguration;

        public static MailClientConfiguration Settings
        {
            get
            {
                if (_mailClientConfiguration == null)
                {
                    _mailClientConfiguration = (MailClientConfiguration)ConfigurationManager.GetSection("molimentum.tasks.mailClient");
                }

                return _mailClientConfiguration;
            }
        }

        [ConfigurationProperty("secret", IsRequired = true)]
        public MailClientSecretSetting Secret
        {
            get
            {
                return (MailClientSecretSetting)this["secret"];
            }
        }

        [ConfigurationProperty("pictures", IsRequired = true)]
        public MailClientPicturesSetting Pictures
        {
            get
            {
                return (MailClientPicturesSetting)this["pictures"];
            }
        }
    }
}