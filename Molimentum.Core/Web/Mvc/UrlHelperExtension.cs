using System.Web.Mvc;

namespace Molimentum.Web.Mvc
{
    public static class UrlHelperExtension
    {
        public static string AbsoluteAction(this UrlHelper url, string action, string controller)
        {
            return AbsoluteAction(url, action, controller, null);
        }

        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues)
        {
            var requestUrl = url.RequestContext.HttpContext.Request.Url;

            var absoluteAction = string.Format("{0}://{1}{2}",
                                                  requestUrl.Scheme,
                                                  requestUrl.Authority,
                                                  url.Action(action, controller, routeValues));

            return absoluteAction;
        }

        public static string Action(this UrlHelper url, string action, string controller, string anchor, object routeValues)
        {
            return string.Format("{0}#{1}", url.Action(action, controller, routeValues), anchor);
        }
    }
}
