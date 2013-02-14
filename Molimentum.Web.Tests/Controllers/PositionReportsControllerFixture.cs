using System;
using System.Web;
using System.Web.Mvc;
using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using Molimentum.Web.Models;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class PositionReportControllerFixture : ControllerFixtureBase<PositionReportsController>
    {
        private IPositionReportRepository _positionReportRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _positionReportRepositoryMock = MockRepository.StrictMock<IPositionReportRepository>();
        }


        protected override PositionReportsController CreateTestedController()
        {
            return new PositionReportsController(_positionReportRepositoryMock);
        }


        [Test]
        public void DefaultPageSizeTest()
        {
            var editableItemController = TestedController as ItemControllerBase;

            Assert.That(editableItemController.DefaultPageSize, Is.EqualTo(10));
        }


        [Test, Sequential]
        public void IndexTest(
            [Values(10, 10, 10, 10, 10)] int pages,
            [Values(null, 10, 12, null, 10, 12)] int? requestedPage,
            [Values(1, 10, 10, 1, 10, 10)] int expectedPage)
        {
            var expectedPageSize = TestedController.DefaultPageSize;
            var expectedRequestedPage = requestedPage == null ? 1 : requestedPage.Value;

            var positionReportListPage = new PagedList<PositionReport>(new PositionReport [] { }, expectedPage, expectedPageSize, pages);

            _positionReportRepositoryMock.Expect(r => r.List(expectedRequestedPage, expectedPageSize)).Return(positionReportListPage);


            var result = TestedController.Index(requestedPage);


            VerifyViewResult(result, "List", typeof(PagedList<PositionReport>), positionReportListPage);
        }


        [Test, Sequential]
        public void KmlTest(
            [Values(1, 10, 0, null)] int? requestedPageSize,
            [Values(1, 10, 0, int.MaxValue)] int? expectedPageSize,
            [Values("xml", "kml", "foo", null)] string extension,
            [Values(KmlMode.Xml, KmlMode.Kml, KmlMode.Kml, KmlMode.Kml)] KmlMode expectedKmlMode)
        {
            var positionReportListPage = new PagedList<PositionReport>(new PositionReport [] { }, 1, expectedPageSize.Value, 1);

            _positionReportRepositoryMock.Expect(r => r.List(1, expectedPageSize.Value)).Return(positionReportListPage);


            var result = TestedController.Kml(requestedPageSize, extension, null, null);


            VerifyViewResult(result, "Kml", typeof(PositionReportKmlData));
            var resultModel = (PositionReportKmlData)(((ViewResult)result).ViewData).Model;
            Assert.That(resultModel.KmlMode, Is.EqualTo(expectedKmlMode));
            Assert.That(resultModel.PositionReports, Is.EqualTo(positionReportListPage));
        }


        [Test, Sequential]
        public void KmlWithDatesTest(
            [Values(1, 10, 0, null)] int? requestedPageSize,
            [Values(1, 10, 0, int.MaxValue)] int? expectedPageSize,
            [Values("xml", "kml", "foo", null)] string extension,
            [Values(KmlMode.Xml, KmlMode.Kml, KmlMode.Kml, KmlMode.Kml)] KmlMode expectedKmlMode)
        {
            var expectedDateFrom = new DateTime(2010, 1, 1);
            var expectedDateTo = new DateTime(2010, 1, 2);
            var positionReportListPage = new PagedList<PositionReport>(new PositionReport[] { }, 1, expectedPageSize.Value, 1);

            _positionReportRepositoryMock.Expect(r => r.ListPublishedByDate(1, expectedPageSize.Value, expectedDateFrom, expectedDateTo)).Return(positionReportListPage);


            var result = TestedController.Kml(requestedPageSize, extension, expectedDateFrom, expectedDateTo);


            VerifyViewResult(result, "Kml", typeof(PositionReportKmlData));
            var resultModel = (PositionReportKmlData)(((ViewResult)result).ViewData).Model;
            Assert.That(resultModel.KmlMode, Is.EqualTo(expectedKmlMode));
            Assert.That(resultModel.PositionReports, Is.EqualTo(positionReportListPage));
        }

        
        [Test]
        public void AddTest()
        {
            var positionReport = new PositionReport();

            AddRole("Author");
            _positionReportRepositoryMock.Expect(r => r.Create()).Return(positionReport);


            var result = TestedController.Add();


            VerifyViewResult(result, "Edit", typeof(EditPositionReportData));
            var resultModel = (EditPositionReportData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Add));
            Assert.That(resultModel.PositionReport, Is.EqualTo(positionReport));
        }


        [Test]
        public void EditTest()
        {
            var positionReportID = "TestPositionReportID";

            var positionReport = new PositionReport();
            positionReport.ID = positionReportID;

            AddRole("Author");
            _positionReportRepositoryMock.Expect(r => r.Get(positionReportID)).Return(positionReport);


            var result = TestedController.Edit(positionReportID);


            VerifyViewResult(result, "Edit", typeof(EditPositionReportData));
            var resultModel = (EditPositionReportData)((ViewResult)result).ViewData.Model;
            Assert.That(resultModel.EditMode, Is.EqualTo(EditMode.Edit));
            Assert.That(resultModel.PositionReport, Is.EqualTo(positionReport));
        }


        [Test]
        public void EditNotFoundTest()
        {
            var positionReportID = "TestPositionReportID";

            AddRole("Author");
            _positionReportRepositoryMock.Expect(r => r.Get(positionReportID)).Return(null);


            try
            {
                var result = TestedController.Edit(positionReportID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        [Test, Sequential]
        public void SaveNewTest(
            [Values(15.0, null)] double? latitude,
            [Values(42.0, null)] double? longitude,
            [Values(false, true)] bool setPositionDateTime)
        {
            var editMode = EditMode.Add;

            var positionDateTime = setPositionDateTime ? new DateTime?(new DateTime(2001, 1, 1)) : null;
            var positionReportID = Guid.Empty.ToString();
            var comment = "A position report comment";
            var position = latitude != null && longitude != null ? new Position(latitude.Value, longitude.Value) : null;

            var positionReport = new PositionReport();
            positionReport.ID = positionReportID;


            _positionReportRepositoryMock.Expect(r => r.Create()).Return(positionReport);
            _positionReportRepositoryMock.Expect(r => r.Save(positionReport));
            _positionReportRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPositionReport(positionReport, positionReportID, positionDateTime, comment, position));


            var result = TestedController.Save(editMode, positionReportID, positionDateTime, comment, latitude, longitude);


            VerifyRedirectToRouteResult(result, expectedAction: "Index");
        }


        [Test, Sequential]
        public void SaveExistingTest(
            [Values(15.0, null)] double? latitude,
            [Values(42.0, null)] double? longitude,
            [Values(false, true)] bool setPositionDateTime)
        {
            var editMode = EditMode.Edit;

            var positionDateTime = setPositionDateTime ? new DateTime?(new DateTime(2001, 1, 1)) : null;
            var positionReportID = "TestPositionReportID";
            var comment = "A position report comment";
            var position = latitude != null && longitude != null ? new Position(latitude.Value, longitude.Value) : null;


            var positionReport = new PositionReport();
            positionReport.ID = positionReportID;
            positionReport.Comment = "";


            _positionReportRepositoryMock.Expect(r => r.Get(positionReportID)).Return(positionReport);
            _positionReportRepositoryMock.Expect(
                r => r.SubmitChanges())
            .WhenCalled(
                b => VerifyPositionReport(positionReport, positionReportID, positionDateTime, comment, position));

            var result = TestedController.Save(editMode, positionReportID, positionDateTime, comment, latitude, longitude);


            VerifyRedirectToRouteResult(result, expectedAction: "Index");
        }


        [Test]
        public void SaveWithInvalidEditModeTest()
        {
            var editMode = (EditMode)Int32.MaxValue;

            var positionReportID = Guid.Empty.ToString();
            var comment = "A position report comment";

            var positionReport = new PositionReport();
            positionReport.ID = positionReportID;


            try
            {
                var result = TestedController.Save(editMode, positionReportID, null, comment, null, null);
                
                Assert.Fail("ArgumentException not raised.");
            }
            catch (ArgumentException ex)
            {
                Assert.That(ex.ParamName, Is.EqualTo("editMode"));
            }
        }
        
        
        [Test]
        public void DeleteTest()
        {
            var positionReportID = "TestPositionReportID";

            var positionReport = new PositionReport();
            positionReport.ID = positionReportID;


            _positionReportRepositoryMock.Expect(r => r.Get(positionReportID)).Return(positionReport);


            var result = TestedController.Delete(positionReportID);


            VerifyViewResult(result, "ConfirmDelete", typeof(PositionReport), positionReport);
        }


        [Test]
        public void SaveDeleteTest()
        {
            var positionReportID = "TestPositionReportID";

            var positionReport = new PositionReport();
            positionReport.ID = positionReportID;


            _positionReportRepositoryMock.Expect(r => r.Get(positionReportID)).Return(positionReport);
            _positionReportRepositoryMock.Expect(r => r.Delete(positionReport));
            _positionReportRepositoryMock.Expect(r => r.SubmitChanges());


            var result = TestedController.SaveDelete(positionReportID);


            VerifyRedirectToRouteResult(result, expectedAction: "Index");
        }


        [Test]
        public void SaveDeleteNotFoundTest()
        {
            var positionReportID = "TestPositionReportID";


            _positionReportRepositoryMock.Expect(r => r.Get(positionReportID)).Return(null);


            try
            {
                var result = TestedController.SaveDelete(positionReportID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        
        [Test]
        public void MapTest()
        {
            var result = TestedController.Map();

            VerifyViewResult(result, "Map", null);
        }
        
        
        private static void VerifyPositionReport(PositionReport positionReportToVerify, string positionReportID,
            DateTime? positionDateTime, string comment, Position position)
        {
            Assert.AreEqual(positionReportToVerify.ID, positionReportID, "PositionReport ID has been modified.");
            Assert.AreEqual(positionReportToVerify.PositionDateTime, positionDateTime, "PositionDateTime not set.");
            Assert.AreEqual(positionReportToVerify.Comment, comment, "Comment not set.");
            Assert.AreEqual(positionReportToVerify.Position, position, "Position not set.");
        }
    }
}