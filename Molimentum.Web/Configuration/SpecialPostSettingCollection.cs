using System.Collections.Generic;
using System.Configuration;

namespace Molimentum.Web.Configuration
{
    public class SpecialPostSettingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SpecialPostSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SpecialPostSetting)element).AlternativeUrl;
        }

        protected override string ElementName
        {
            get
            {
                return "specialPost";
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public IEnumerable<SpecialPostSetting> Settings
        {
            get
            {
                foreach (var specialPostSetting in this)
                {
                    yield return (SpecialPostSetting)specialPostSetting;
                }
            }
        }
    }
}
