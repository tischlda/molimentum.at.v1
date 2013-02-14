using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class PicasaPicturesSetting : ConfigurationElement
    {
        [ConfigurationProperty("thumbnailSize", DefaultValue = 144)]
        public int ThumbnailSize
        {
            get
            {
                return (int)this["thumbnailSize"];
            }
        }

        [ConfigurationProperty("maximumImageSize", DefaultValue = null)]
        public int? MaximumImageSize
        {
            get
            {
                return this["maximumImageSize"] == null ? null : (int?)this["maximumImageSize"];
            }
        }
    }
}