using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Molimentum.Web.Mvc
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;

            if (!typeof(IController).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException(String.Format("Type requested is not a controller: {0}", controllerType.Name), "controllerType");
            }

            return _container.Resolve(controllerType) as IController;
        }
    }
}