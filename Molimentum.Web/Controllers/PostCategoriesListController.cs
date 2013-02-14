using System.Web.Mvc;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class PostCategoriesListController : WidgetControllerBase
    {
        private readonly IPostCategoryRepository _postCategoryRepository;

        public PostCategoriesListController(IPostCategoryRepository postCategoryRepository)
        {
            _postCategoryRepository = postCategoryRepository;
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public override ActionResult Widget()
        {
            var postCategories = _postCategoryRepository.List(1, int.MaxValue);

            return View("PostCategoriesListWidget", postCategories.Items);
        }
    }
}