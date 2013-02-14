using System.Web.Mvc;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class ShopController : PrimaryControllerBase
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}