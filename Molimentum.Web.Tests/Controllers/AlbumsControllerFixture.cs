using System.Web;
using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class AlbumsControllerFixture : ControllerFixtureBase<AlbumsController>
    {
        private IPictureRepository _pictureRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _pictureRepositoryMock = MockRepository.StrictMock<IPictureRepository>();
        }


        protected override AlbumsController CreateTestedController()
        {
            return new AlbumsController(_pictureRepositoryMock);
        }
        
        
        [Test]
        public void DefaultPageSizeTest()
        {
            var editableItemController = TestedController as ItemControllerBase;
            
            Assert.That(editableItemController.DefaultPageSize, Is.EqualTo(10));
        }


        [Test, Sequential]
        public void DetailTest(
            [Values("text", "map", null)] string mode,
            [Values("DetailText", "DetailMap", "Detail")] string expectedView)
        {
            var albumID = "TestAlbumID";

            var albumSummary = new AlbumSummary();
            albumSummary.ID = albumID;

            var album = new Album();
            album.Summary = albumSummary;


            _pictureRepositoryMock.Expect(r => r.GetAlbum(albumID)).Return(album);


            var result = TestedController.Detail(albumID, mode);


            VerifyViewResult(result, expectedView, typeof(Album), album);
        }

        [Test]
        public void DetailNotFoundTest()
        {
            var albumID = "TestAlbumID";


            _pictureRepositoryMock.Expect(r => r.GetAlbum(albumID)).Return(null);


            try
            {
                var result = TestedController.Detail(albumID, null);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        
        [Test, Sequential]
        public void IndexTest(
            [Values(10, 10, 10, 10, 10, 10)] int pages,
            [Values(null, 10, 12, null, 10, 12)] int? requestedPage,
            [Values(1, 10, 10, 1, 10, 10)] int expectedPage,
            [Values(null, "", "text", "foo", null, null)] string mode,
            [Values("List", "List", "ListText", "List", "List", "List")] string expectedView)
        {
            var expectedPageSize = TestedController.DefaultPageSize;
            var expectedRequestedPage = requestedPage == null ? 1 : requestedPage.Value;

            var albumListPage = new PagedList<AlbumSummary>(new AlbumSummary[] { }, expectedPage, expectedPageSize, pages);

            _pictureRepositoryMock.Expect(r => r.ListAlbums(expectedRequestedPage, expectedPageSize)).Return(albumListPage);


            var result = TestedController.Index(requestedPage, mode);


            VerifyViewResult(result, expectedView, typeof(PagedList<AlbumSummary>), albumListPage);
        }
    }
}