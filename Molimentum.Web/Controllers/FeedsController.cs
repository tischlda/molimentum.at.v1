using System.IO;
using System.Web.Mvc;
using System.Xml;
using Molimentum.Model;
using Molimentum.Services;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class FeedsController : Controller
    {
        private readonly IFeedService _feedService;

        public FeedsController(IFeedService feedService)
        {
            _feedService = feedService;
        }

        public ActionResult GetFeed(FeedFormat format)
        {
            using (var stringWriter = new StringWriter())
            {
                var xmlWriter = new XmlTextWriter(stringWriter);

                _feedService.CreateFeed(format, Url).WriteTo(xmlWriter);

                return Content(stringWriter.ToString(), "text/xml");
            }
        }
    }
}