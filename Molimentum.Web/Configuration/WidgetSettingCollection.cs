using System.Collections.Generic;
using System.Configuration;

namespace Molimentum.Web.Configuration
{
    public class WidgetSettingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new WidgetSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WidgetSetting)element).Name;
        }

        protected override string ElementName
        {
            get
            {
                return "widget";
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public IEnumerable<WidgetSetting> Settings
        {
            get
            {
                foreach (var widgetSetting in this)
                {
                    yield return (WidgetSetting)widgetSetting;
                }
            }
        }
    }
}
