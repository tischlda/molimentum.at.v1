using System.Configuration;

namespace Molimentum.Tasks.Configuration
{
    public class TasksConfiguration : ConfigurationSection
    {
        private static TasksConfiguration _TasksConfiguration;

        public static TasksConfiguration Settings
        {
            get
            {
                if (_TasksConfiguration == null)
                {
                    _TasksConfiguration = (TasksConfiguration)ConfigurationManager.GetSection("molimentum.tasks");
                }

                return _TasksConfiguration;
            }
        }

        [ConfigurationProperty("tasks")]
        public TaskSettingCollection Tasks
        {
            get
            {
                return (TaskSettingCollection)this["tasks"];
            }
        }
    }
}