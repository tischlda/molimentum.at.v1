using System.Web.Mvc;

namespace Molimentum.Web.Controllers
{
    public abstract class EditableItemControllerBase : ItemControllerBase
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewData["Editable"] = HttpContext.User.IsInRole("Author") || HttpContext.User.IsInRole("Administrator");
        }
    }
}
