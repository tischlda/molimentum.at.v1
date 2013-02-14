using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace Molimentum.Web.Mvc
{
    public static class IncludeExtensions
    {
        private const string c_clientScriptInclude = "<script type='text/javascript' src='{0}'></script>\n";
        private const string c_styleSheetInclude = "<link rel='stylesheet' type='text/css' href='{0}'/>\n";


        public static string ClientScriptIncludes(this HtmlHelper html, Page page)
        {
            return ClientScriptIncludes(html, page, page);

        }

        public static string ClientScriptIncludes(this HtmlHelper html, MasterPage masterPage)
        {
            return ClientScriptIncludes(html, masterPage, masterPage, masterPage.Page);
        }

        private static string ClientScriptIncludes(this HtmlHelper html, IUrlResolutionService urlResolver, params object[] attributedObjects)
        {
            return RenderIncludes(
                html,
                typeof(ClientScriptIncludeAttribute),
                urlResolver,
                c_clientScriptInclude,
                attributedObjects);
        }

        
        public static string StyleSheetIncludes(this HtmlHelper html, Page page)
        {
            return StyleSheetIncludes(html, page, page);

        }

        public static string StyleSheetIncludes(this HtmlHelper html, MasterPage masterPage)
        {
            return StyleSheetIncludes(html, masterPage, masterPage, masterPage.Page);
        }

        private static string StyleSheetIncludes(this HtmlHelper html, IUrlResolutionService urlResolver, params object[] attributedObjects)
        {
            return RenderIncludes(
                html,
                typeof(StyleSheetIncludeAttribute),
                urlResolver,
                c_styleSheetInclude,
                attributedObjects);
        }

        private static string RenderIncludes(HtmlHelper html, Type includeAttributeType, IUrlResolutionService urlResolver, string format, params object[] attributedObjects)
        {
            var includeUrls = new List<string>();

            foreach (var attributedObject in attributedObjects)
            {
                foreach (var includeAttribute in attributedObject.GetType().GetCustomAttributes(includeAttributeType, true).Cast<IncludeAttribute>())
                {
                    foreach (var url in includeAttribute.Urls)
                    {
                        var resolvedUrl = urlResolver.ResolveClientUrl(url);
                        
                        if (!includeUrls.Contains(resolvedUrl)) includeUrls.Add(resolvedUrl);
                    }
                }
            }

            var includesBuilder = new StringBuilder();

            foreach (var url in includeUrls)
            {
                includesBuilder.AppendFormat(format, url);
            }

            return includesBuilder.ToString();
        }

        //public static string ClientScriptIncludes(this HtmlHelper html, Page page)
        //{
        //    var includesBuilder = new StringBuilder();

        //    RenderClientScriptIncludes(page, includesBuilder, page);

        //    return includesBuilder.ToString();
        //}

        //public static string ClientScriptIncludes(this HtmlHelper html, MasterPage masterPage)
        //{
        //    var includesBuilder = new StringBuilder();

        //    RenderClientScriptIncludes(masterPage, includesBuilder, masterPage);
        //    RenderClientScriptIncludes(masterPage.Page, includesBuilder, masterPage);

        //    foreach(var viewDataItem in html.ViewData.Values)
        //    {
        //        RenderClientScriptIncludes(viewDataItem, includesBuilder, "<script type='text/javascript' src='{0}'></script>\n", masterPage);
        //    }

        //    return includesBuilder.ToString();
        //}

        //    private static void RenderClientScriptIncludes(object o, StringBuilder includesBuilder, string format, IUrlResolutionService urlResolver)
        //    {
        //        foreach (var clientScriptAttribute in o.GetType().GetCustomAttributes(typeof(ClientScriptIncludeAttribute), true).Cast<ClientScriptIncludeAttribute>())
        //        {
        //            foreach (var url in clientScriptAttribute.Urls)
        //            {
        //                includesBuilder.AppendFormat(format, urlResolver.ResolveClientUrl(url));
        //            }
        //        }
        //    }


        //    public static string StyleSheetIncludes(this HtmlHelper html, Page page)
        //    {
        //        var includesBuilder = new StringBuilder();

        //        RenderStyleSheetIncludes(page, includesBuilder, page);

        //        return includesBuilder.ToString();
        //    }

        //    public static string StyleSheetIncludes(this HtmlHelper html, MasterPage masterPage)
        //    {
        //        var includesBuilder = new StringBuilder();

        //        RenderStyleSheetIncludes(masterPage, includesBuilder, masterPage);
        //        RenderStyleSheetIncludes(masterPage.Page, includesBuilder, masterPage);

        //        foreach (var viewDataItem in html.ViewData.Values)
        //        {
        //            RenderClientScriptIncludes(viewDataItem, includesBuilder, masterPage);
        //        }

        //        return includesBuilder.ToString();
        //    }

        //    private static void RenderStyleSheetIncludes(object o, StringBuilder includesBuilder, IUrlResolutionService urlResolver)
        //    {
        //        foreach (var StyleSheetAttribute in o.GetType().GetCustomAttributes(typeof(StyleSheetIncludeAttribute), true).Cast<StyleSheetIncludeAttribute>())
        //        {
        //            foreach (var url in StyleSheetAttribute.Urls)
        //            {
        //                includesBuilder.AppendFormat("<link rel='stylesheet' type='text/css' href='{0}'/>\n", urlResolver.ResolveClientUrl(url));
        //            }
        //        }
        //    }
        //}
    }
}