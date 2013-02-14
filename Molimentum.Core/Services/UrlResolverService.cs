using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Molimentum.Model;
using Molimentum.Services;
using Molimentum.Web.Mvc;

namespace Molimentum.Services
{
    public class UrlResolverService : IUrlResolverService
    {
        private static readonly Dictionary<Type, string> s_typeToControllerMapping = new Dictionary<Type, string>();

        public static void MapTypeToController(Type type, string controller)
        {
            s_typeToControllerMapping.Add(type, controller);
        }

        public Uri CreateDetailLink(UrlHelper urlHelper, IItem item)
        {
            return CreateDetailLink(urlHelper, item, false);
        }

        public Uri CreateDetailLink(UrlHelper urlHelper, IItem item, bool absolute)
        {
            if (!s_typeToControllerMapping.ContainsKey(item.GetType()))
            {
                throw new ArgumentException(String.Format("No controller mapped to type {0}.", item.GetType()));
            }

            var controller = s_typeToControllerMapping[item.GetType()];

            return new Uri(
                absolute ?
                urlHelper.AbsoluteAction("Detail", controller, new { id = item.ID }) :
                urlHelper.Action("Detail", controller, new { id = item.ID }));
        }
    }
}