using System.Configuration;

namespace Molimentum.Tasks.MailClient.Configuration
{
    public class MailClientPicturesSetting : ConfigurationElement
    {
        [ConfigurationProperty("albumID", IsRequired = true)]
        public string AlbumID
        {
            get
            {
                return (string)this["albumID"];
            }
        }
    }
}