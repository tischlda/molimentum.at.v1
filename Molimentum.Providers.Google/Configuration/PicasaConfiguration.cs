using System.Configuration;

namespace Molimentum.Providers.Google.Configuration
{
    public class PicasaConfiguration : ConfigurationSection
    {
        private static PicasaConfiguration _picasaConfiguration;

        public static PicasaConfiguration Settings
        {
            get
            {
                if (_picasaConfiguration == null)
                {
                    _picasaConfiguration = (PicasaConfiguration)ConfigurationManager.GetSection("molimentum.providers.google.picasa");
                }

                return _picasaConfiguration;
            }
        }

        [ConfigurationProperty("authentication", IsRequired = true)]
        public PicasaAuthenticationSetting Authentication
        {
            get
            {
                return (PicasaAuthenticationSetting) this["authentication"];
            }
        }

        [ConfigurationProperty("application", IsRequired = true)]
        public PicasaApplicationSetting Application
        {
            get
            {
                return (PicasaApplicationSetting)this["application"];
            }
        }

        [ConfigurationProperty("gallery", IsRequired = true)]
        public PicasaGallerySetting Gallery
        {
            get
            {
                return (PicasaGallerySetting)this["gallery"];
            }
        }

        [ConfigurationProperty("pictures", IsRequired = true)]
        public PicasaPicturesSetting Pictures
        {
            get
            {
                return (PicasaPicturesSetting)this["pictures"];
            }
        }
    }
}
