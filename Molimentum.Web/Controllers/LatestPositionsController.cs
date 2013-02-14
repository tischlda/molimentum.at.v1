using System.Linq;
using System.Web.Mvc;
using Molimentum.Repositories;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class LatestPositionsController : WidgetControllerBase
    {
        private readonly IPositionReportRepository _positionReportRepository;

        public LatestPositionsController(IPositionReportRepository positionReportRepository)
        {
            _positionReportRepository = positionReportRepository;
        }

        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public override ActionResult Widget()
        {
            var page = 1;
            var pageSize = 1;

            var positionReport = _positionReportRepository.ListPublished(page, pageSize).Items.First();

            return View("LatestPositionsWidget", positionReport);
        }
    }
}