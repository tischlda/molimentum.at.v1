using System.Web.Mvc;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class LatestPostsController : WidgetControllerBase
    {
        private readonly IPostRepository _postRepository;

        public LatestPostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public override ActionResult Widget()
        {
            var page = 1;
            var pageSize = 10;

            var postListPage = _postRepository.ListPublished(page, pageSize);

            return View("LatestPostsWidget", postListPage);
        }
    }
}