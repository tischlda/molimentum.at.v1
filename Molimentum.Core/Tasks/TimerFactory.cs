using System.Timers;

namespace Molimentum.Tasks
{
    public class TimerFactory
    {
        public virtual Timer CreateTimer()
        {
            return new Timer();
        }
    }
}
