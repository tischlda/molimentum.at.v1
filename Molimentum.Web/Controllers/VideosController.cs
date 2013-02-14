using System.Web;
using System.Web.Mvc;
using Molimentum.Model;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class VideosController : ItemControllerBase
    {
        private readonly IVideoRepository _videoRepository;

        public VideosController(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var pageSize = DefaultPageSize;

            var videos = _videoRepository.ListVideos(page.Value, pageSize);

            return View("List", videos);
        }

        public ActionResult Detail(string id)
        {
            var video = LoadVideo(id);

            return View("Detail", video);
        }

        private Video LoadVideo(string id)
        {
            var video = _videoRepository.GetVideo(id);

            if (video == null) throw new HttpException(404, "Video not found.");

            return video;
        }
    }
}