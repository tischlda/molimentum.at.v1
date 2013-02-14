using System.Web.Mvc;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class TranslateController : WidgetControllerBase
    {
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public override ActionResult Widget()
        {
            return View("TranslateWidget");
        }
    }
}