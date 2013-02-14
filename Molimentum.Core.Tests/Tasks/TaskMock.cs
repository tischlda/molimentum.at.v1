namespace Molimentum.Tasks.Tests
{
    public class TaskMock : ITask
    {
        public void Execute()
        {
            Executed = true;
        }

        public bool Executed { get; set; }
    }
}
