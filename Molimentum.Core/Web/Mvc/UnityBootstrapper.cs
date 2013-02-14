using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity;
using System.Web;
using System.Configuration;

namespace Molimentum.Web.Mvc
{
    public static class UnityBootstrapper
    {
        public static IUnityContainer Container { get; private set; }

        public static void Initialize(HttpContext context)
        {
            Container = new UnityContainer();

            Container.RegisterInstance(context.Cache);

            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers["mvc"].Configure(Container);
        }
    }
}
