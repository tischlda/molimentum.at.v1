using System.Configuration;

namespace Molimentum.Tasks.Configuration
{
    public class TaskSettingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TaskSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.GetHashCode();
        }

        public TaskSetting this[int index]
        {
            get
            {
                return (TaskSetting)BaseGet(index);
            }
        }

        protected override string ElementName
        {
            get
            {
                return "task";
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
    }
}