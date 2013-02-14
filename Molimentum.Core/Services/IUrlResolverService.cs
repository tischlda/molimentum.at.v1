using System;
using System.Web.Mvc;
using Molimentum.Model;

namespace Molimentum.Services
{
    public interface IUrlResolverService
    {
        Uri CreateDetailLink(UrlHelper urlHelper, IItem item);
        Uri CreateDetailLink(UrlHelper urlHelper, IItem item, bool absolute);
    }
}