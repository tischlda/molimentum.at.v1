using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Molimentum.Model;
using Molimentum.Tasks;
using Molimentum.Web.Configuration;
using Molimentum.Web.Mvc;
using Molimentum.Services;

namespace Molimentum.Web
{
    public class MvcApplication : HttpApplication
    {
        private TaskRunner _taskRunner;

        protected void Application_Start()
        {
            UnityBootstrapper.Initialize(HttpContext.Current);

            RegisterControllerFactory();
            RegisterRoutes();
            RegisterUrlResolver();

            // databinding bug workaround (http://forums.asp.net/t/1632006.aspx)
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();

            _taskRunner = new TaskRunner(UnityBootstrapper.Container);
            _taskRunner.Start();
        }

        protected void Application_Stop()
        {
            _taskRunner.Stop();
        }


        public override void Dispose()
        {
            if (_taskRunner != null) _taskRunner.Dispose();

            base.Dispose();
        }

        private void RegisterControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(UnityBootstrapper.Container));
        }

        private static void RegisterRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            foreach (var specialPost in WebConfiguration.Settings.SpecialPosts.Settings)
            {
                RouteTable.Routes.MapRoute(
                    "SpecialPost_" + specialPost.AlternativeUrl,
                    specialPost.AlternativeUrl,
                    new { controller = "Posts", action = "Detail", id = specialPost.Id }
                );
            }

            RouteTable.Routes.MapRoute(
                "Search",
                "Search",
                new { controller = "Search", action = "Search" }
            );

            RouteTable.Routes.MapRoute(
                "Feeds",
                "Feed/{format}",
                new { controller = "Feeds", action = "GetFeed", format = FeedFormat.Rss }
            );

            RouteTable.Routes.MapRoute(
                "Tags",
                "Tags/{tag}",
                new { controller = "Posts", action = "Query" }
            );

            RouteTable.Routes.MapRoute(
                "Categories",
                "Categories/{title}",
                new { controller = "PostCategories", action = "DetailByTitle" }
            );

            RouteTable.Routes.MapRoute(
                "Index",
                "{controller}",
                new { controller = "Posts", action = "Index" }
            );

            RouteTable.Routes.MapRoute(
                "SpecialFilenames",
                "{controller}/{action}.{extension}"
            );

            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Posts", action = "Index", id = "" }
            );
        }

        private static void RegisterUrlResolver()
        {
            UrlResolverService.MapTypeToController(typeof(AlbumSummary), "Albums");
            UrlResolverService.MapTypeToController(typeof(Video), "Videos");
            UrlResolverService.MapTypeToController(typeof(Post), "Posts");
            UrlResolverService.MapTypeToController(typeof(PositionReport), "PositionReports");
            UrlResolverService.MapTypeToController(typeof(PostCategory), "PostCategories");
            UrlResolverService.MapTypeToController(typeof(PostComment), "PostComments");
        }
    }
}