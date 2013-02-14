using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class LatestPositionsControllerFixture : ControllerFixtureBase<LatestPositionsController>
    {
        private IPositionReportRepository _positionReportRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _positionReportRepositoryMock = MockRepository.StrictMock<IPositionReportRepository>();
        }


        protected override LatestPositionsController CreateTestedController()
        {
            return new LatestPositionsController(_positionReportRepositoryMock);
        }
        
        
        [Test]
        public void WidgetTest()
        {
            var positionReport = new PositionReport();
            var positionReportListPage = new PagedList<PositionReport>(new [] { positionReport }, 1, 1, 1);

            _positionReportRepositoryMock.Expect(r => r.ListPublished(1, 1)).Return(positionReportListPage);


            var result = TestedController.Widget();


            VerifyViewResult(result, "LatestPositionsWidget", typeof(PositionReport), positionReport);
        }
    }
}