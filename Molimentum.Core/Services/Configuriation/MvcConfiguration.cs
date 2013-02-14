using System.Configuration;

namespace Molimentum.Services.Configuration
{
    public class MvcConfiguration : ConfigurationSection
    {
        private static MvcConfiguration _mvcConfiguration;

        public static MvcConfiguration Settings
        {
            get
            {
                if (_mvcConfiguration == null)
                {
                    _mvcConfiguration = (MvcConfiguration)ConfigurationManager.GetSection("molimentum.services");
                }

                return _mvcConfiguration;
            }
        }

        [ConfigurationProperty("emailNotification", IsRequired = false)]
        public EMailNotificationSetting EMailNotification
        {
            get
            {
                return (EMailNotificationSetting)this["emailNotification"];
            }
        }
    }
}