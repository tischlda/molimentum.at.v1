using System.Configuration;

namespace Molimentum.Web.Configuration
{
    public class WidgetSetting : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("adminOnly", IsRequired = false, DefaultValue = false)]
        public bool AdminOnly
        {
            get
            {
                return (bool)this["adminOnly"];
            }
        }
    }
}
