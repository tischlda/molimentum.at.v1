using System.Web.Mvc;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class LatestAlbumsController : WidgetControllerBase
    {
        private readonly IPictureRepository _pictureRepository;

        public LatestAlbumsController(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        [OutputCache(Duration = 60, VaryByParam = "none")]
        public override ActionResult Widget()
        {
            var page = 1;
            var pageSize = 5;

            var albumListPage = _pictureRepository.ListAlbums(page, pageSize);

            return View("LatestAlbumsWidget", albumListPage);
        }
    }
}