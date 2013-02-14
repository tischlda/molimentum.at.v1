using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace Molimentum.Web.Mvc
{
    public class HttpContextLifetimeManager : LifetimeManager, IDisposable
    {
        private readonly Guid _key = Guid.NewGuid();

        public override object GetValue()
        {
            return HttpContext.Current.Items[_key];
        }

        public override void RemoveValue()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(_key))
                {
                    var value = HttpContext.Current.Items[_key] as IDisposable;

                    if (value != null) value.Dispose();

                    HttpContext.Current.Items.Remove(_key);
                }
            }
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[_key] = newValue;
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
                RemoveValue();
            }

            _disposed = true;
        }

        ~HttpContextLifetimeManager()
        {
            Dispose(false);
        }
    }
}