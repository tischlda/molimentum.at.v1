using System.Configuration;

namespace Molimentum.Web.Configuration
{
    public class WebConfiguration : ConfigurationSection
    {
        private static WebConfiguration _webConfiguration;

        public static WebConfiguration Settings
        {
            get
            {
                if (_webConfiguration == null)
                {
                    _webConfiguration = (WebConfiguration)ConfigurationManager.GetSection("molimentum.web");
                }

                return _webConfiguration;
            }
        }

        [ConfigurationProperty("widgets", IsRequired = false)]
        public WidgetSettingCollection Widgets
        {
            get
            {
                return (WidgetSettingCollection)this["widgets"];
            }
        }

        [ConfigurationProperty("specialPosts", IsRequired = false)]
        public SpecialPostSettingCollection SpecialPosts
        {
            get
            {
                return (SpecialPostSettingCollection)this["specialPosts"];
            }
        }
    }
}