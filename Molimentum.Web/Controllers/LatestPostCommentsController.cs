using System.Web.Mvc;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class LatestPostCommentsController : WidgetControllerBase
    {
        private readonly IPostCommentRepository _postCommentRepository;

        public LatestPostCommentsController(IPostCommentRepository postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;
        }

        public override ActionResult Widget()
        {
            var page = 1;
            var pageSize = 10;

            var postListPage = _postCommentRepository.ListPublished(page, pageSize);

            return View("LatestPostCommentsWidget", postListPage);
        }
    }
}