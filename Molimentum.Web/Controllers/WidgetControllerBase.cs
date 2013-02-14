using System.Web.Mvc;

namespace Molimentum.Web.Controllers
{
    [ValidateInput(false)]
    public abstract class WidgetControllerBase : Controller
    {
        [ChildActionOnly]
        public abstract ActionResult Widget();
    }
}