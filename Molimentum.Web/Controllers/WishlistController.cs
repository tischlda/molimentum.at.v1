using System.Web.Mvc;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class WishlistController : WidgetControllerBase
    {
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public override ActionResult Widget()
        {
            return View("WishlistWidget");
        }
    }
}