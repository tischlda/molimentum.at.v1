using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Practices.Unity;
using Molimentum.Tasks.Configuration;

namespace Molimentum.Tasks
{
    public class TaskRunner : IDisposable
    {
        private List<Timer> _timers = new List<Timer>();
        private IUnityContainer _container;

        public TaskRunner(IUnityContainer container)
            : this(container, new TimerFactory())
        {

        }

        public TaskRunner(IUnityContainer container, TimerFactory timerFactory)
        {
            _container = container;

            foreach (TaskSetting taskSetting in TasksConfiguration.Settings.Tasks)
            {
                var task = (ITask)_container.Resolve(taskSetting.Type);

                var timer = timerFactory.CreateTimer();
                timer.Interval = taskSetting.Interval;
                timer.AutoReset = true;
                timer.Elapsed += (sender, e) => task.Execute();

                _timers.Add(timer);
            }
        }

        public void Start()
        {
            foreach(var timer in _timers)
            {
                timer.Start();
            }
        }

        public void Stop()
        {
            foreach (var timer in _timers)
            {
                timer.Stop();
            }
        }


        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // release managed resources
                }

                // release unmanaged resources
                // disposal of injected dependencies is managed by the IOC container
                _container = null;

                foreach (var timer in _timers)
                {
                    timer.Dispose();
                }

                _timers = null;
            }

            _disposed = true;
        }

        ~TaskRunner()
        {
            Dispose(false);
        }
    }
}
