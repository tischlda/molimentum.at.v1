using Microsoft.Practices.Unity;
using NUnit.Framework;
using Rhino.Mocks;
using System.Timers;

namespace Molimentum.Tasks.Tests
{
    [TestFixture]
    public class TaskRunnerFixture
    {
        [Test]
        public void TaskRunnerTest()
        {
            var taskMock = new TaskMock();

            var timerMock = MockRepository.GenerateMock<Timer>();
            timerMock.Expect(m => m.Elapsed += (sender, e) => taskMock.Execute());

            var timerFactoryMock = MockRepository.GenerateMock<TimerFactory>();
            timerFactoryMock.Expect(m => m.CreateTimer()).Return(timerMock);
            
            var containerMock = MockRepository.GenerateMock<IUnityContainer>();
            containerMock.Expect(m => m.Resolve(typeof(TaskMock))).Return(taskMock);

            timerMock.Replay();
            timerFactoryMock.Replay();
            containerMock.Replay();

            using (var taskRunner = new TaskRunner(containerMock))
            {
                taskRunner.Start();

                Assert.That(taskMock.Executed, Is.False);

                timerMock.Raise(m => m.Elapsed += null, new object[] { null, null });

                Assert.That(taskMock.Executed, Is.True);
                taskMock.Executed = false;

                timerMock.Raise(m => m.Elapsed += null, new object[] { null, null });

                Assert.That(taskMock.Executed, Is.True);

                taskRunner.Stop();
            }
        }
    }
}
