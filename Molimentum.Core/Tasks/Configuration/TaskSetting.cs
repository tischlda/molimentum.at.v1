using System;
using System.ComponentModel;
using System.Configuration;

namespace Molimentum.Tasks.Configuration
{
    public class TaskSetting : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        [TypeConverter(typeof(TypeTypeConverter))]
        public Type Type
        {
            get
            {
                return (Type)this["type"];
            }
        }

        [ConfigurationProperty("interval", IsRequired = true)]
        public int Interval
        {
            get
            {
                return (int)this["interval"];
            }
        }
    }
}