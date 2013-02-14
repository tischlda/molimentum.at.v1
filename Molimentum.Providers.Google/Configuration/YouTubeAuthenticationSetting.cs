using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class YouTubeAuthenticationSetting : ConfigurationElement
    {
        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
        }
    }
}