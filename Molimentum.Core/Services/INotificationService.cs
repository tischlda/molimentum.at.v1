namespace Molimentum.Services
{
    public interface INotificationService
    {
        void Notify(string action, object o);
    }
}