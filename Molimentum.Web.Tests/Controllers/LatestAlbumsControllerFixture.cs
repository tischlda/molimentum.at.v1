using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class LatestAlbumsControllerFixture : ControllerFixtureBase<LatestAlbumsController>
    {
        private IPictureRepository _pictureRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _pictureRepositoryMock = MockRepository.StrictMock<IPictureRepository>();
        }


        protected override LatestAlbumsController CreateTestedController()
        {
            return new LatestAlbumsController(_pictureRepositoryMock);
        }
        
        
        [Test]
        [Ignore("Ignored temporary.")]
        public void WidgetTest()
        {
            var albumListPage = new PagedList<AlbumSummary>(new AlbumSummary[] { }, 1, 1, 1);

            _pictureRepositoryMock.Expect(r => r.ListAlbums(1, 1)).Return(albumListPage);


            var result = TestedController.Widget();


            VerifyViewResult(result, "LatestAlbumsWidget", typeof(PagedList<AlbumSummary>), albumListPage);
        }
    }
}