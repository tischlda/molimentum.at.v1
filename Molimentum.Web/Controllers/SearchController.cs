using System.Web.Mvc;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class SearchController : WidgetControllerBase
    {
        private readonly IPostRepository _postRepository;
        
        public SearchController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public override ActionResult Widget()
        {
            var tags = _postRepository.ListTags();

            return View("SearchWidget", tags);
        }

        public ActionResult Search()
        {
            return View();
        }
    }
}