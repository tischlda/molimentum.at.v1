using Molimentum.Tasks.Configuration;
using NUnit.Framework;

namespace Molimentum.Tasks.Tests
{
    [TestFixture]
    public class ConfigurationFixture
    {
        [Test]
        public void ConfigurationTest()
        {
            Assert.That(TasksConfiguration.Settings, Is.Not.Null);
            Assert.That(TasksConfiguration.Settings.Tasks, Is.Not.Null);
            Assert.That(TasksConfiguration.Settings.Tasks.Count, Is.EqualTo(1));
            Assert.That(TasksConfiguration.Settings.Tasks[0].Type, Is.EqualTo(typeof(TaskMock)));
            Assert.That(TasksConfiguration.Settings.Tasks[0].Interval, Is.EqualTo(60000));
        }
    }
}