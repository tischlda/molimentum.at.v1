using System;
using System.Web;
using System.Web.Mvc;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Models;
using Molimentum;

namespace Molimentum.Web.Controllers
{
    [HandleError]
    public class PositionReportsController : EditableItemControllerBase
    {
        private readonly IPositionReportRepository _positionReportRepository;

        public PositionReportsController(IPositionReportRepository positionReportRepository)
        {
            _positionReportRepository = positionReportRepository;
        }

        private PositionReport LoadPositionReport(string id)
        {
            var positionReport = _positionReportRepository.Get(id);

            if (positionReport == null) throw new HttpException(404, "PositionReport not found.");

            return positionReport;
        }

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var pageSize = DefaultPageSize;

            var positionReportListPage = _positionReportRepository.List(page.Value, pageSize);

            return View("List", positionReportListPage);
        }

        public ActionResult Kml(int? pageSize, string extension, DateTime? from, DateTime? to)
        {
            if (pageSize == null) pageSize = int.MaxValue;

            var kmlMode = extension == "xml" ? KmlMode.Xml : KmlMode.Kml;
            
            var positionReportListPage =
                (from != null && to != null)
                ?
                _positionReportRepository.ListPublishedByDate(1, pageSize.Value, from.Value, to.Value)
                :
                _positionReportRepository.List(1, pageSize.Value);

            return View("Kml", new PositionReportKmlData { PositionReports = positionReportListPage, KmlMode = kmlMode });
        }

        public ActionResult Map()
        {
            return View("Map");
        }

        [Authorize]
        public ActionResult Add()
        {
            var editData = new EditPositionReportData
                               {
                                   EditMode = EditMode.Add,
                                   PositionReport = _positionReportRepository.Create()
                               };

            return View("Edit", editData);
        }

        [Authorize(Roles = "Administrator,Author")]
        public ActionResult Edit(string id)
        {
            var editData = new EditPositionReportData
                               {
                                   EditMode = EditMode.Edit,
                                   PositionReport = LoadPositionReport(id)
                               };

            return View("Edit", editData);
        }

        [Authorize(Roles = "Administrator,Author")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(
            EditMode editMode, string id,
            DateTime? positionDateTime, string comment, double? latitude, double? longitude)
        {
            var positionReport = GetPositionReport(editMode, id);

            positionReport.PositionDateTime = positionDateTime;
            positionReport.Comment = comment;
            
            if (latitude != null && longitude != null)
            {
                positionReport.Position = new Position(latitude.Value, longitude.Value);
            }

            if (!ModelState.IsValid)
            {
                var editData = new EditPositionReportData
                {
                    EditMode = editMode,
                    PositionReport = positionReport
                };

                return View("Edit", editData);
            }

            if (editMode == EditMode.Add) _positionReportRepository.Save(positionReport);

            _positionReportRepository.SubmitChanges();

            return RedirectToAction("Index");
        }

        private PositionReport GetPositionReport(EditMode editMode, string id)
        {
            PositionReport positionReport;

            switch (editMode)
            {
                case EditMode.Add:
                    positionReport = _positionReportRepository.Create();
                    break;

                case EditMode.Edit:
                    positionReport = _positionReportRepository.Get(id);
                    break;

                default:
                    throw new ArgumentException(String.Format("Unknown EditMode '{0}'.", editMode), "editMode");
            }

            return positionReport;
        }

        [Authorize(Roles = "Administrator,Author")]
        public ActionResult Delete(string id)
        {
            var positionReport = LoadPositionReport(id);

            return View("ConfirmDelete", positionReport);
        }

        [Authorize(Roles = "Administrator,Author")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveDelete(string id)
        {
            var positionReport = LoadPositionReport(id);

            _positionReportRepository.Delete(positionReport);

            _positionReportRepository.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}