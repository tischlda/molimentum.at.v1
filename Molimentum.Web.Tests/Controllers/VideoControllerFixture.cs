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
    public class VideosControllerFixture : ControllerFixtureBase<VideosController>
    {
        private IVideoRepository _videoRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _videoRepositoryMock = MockRepository.StrictMock<IVideoRepository>();
        }


        protected override VideosController CreateTestedController()
        {
            return new VideosController(_videoRepositoryMock);
        }


        [Test]
        public void DefaultPageSizeTest()
        {
            var editableItemController = TestedController as ItemControllerBase;

            Assert.That(editableItemController.DefaultPageSize, Is.EqualTo(10));
        }


        [Test]
        public void DetailTest()
        {
            var videoID = "TestVideoID";

            var video = new Video();
            video.ID = videoID;


            _videoRepositoryMock.Expect(r => r.GetVideo(videoID)).Return(video);


            var result = TestedController.Detail(videoID);


            VerifyViewResult(result, "Detail", typeof(Video), video);
        }

        [Test]
        public void DetailNotFoundTest()
        {
            var videoID = "TestVideoID";


            _videoRepositoryMock.Expect(r => r.GetVideo(videoID)).Return(null);


            try
            {
                var result = TestedController.Detail(videoID);

                Assert.Fail("404 not raised.");
            }
            catch (HttpException ex)
            {
                Assert.That(ex.GetHttpCode(), Is.EqualTo(404));
            }
        }

        
        [Test, Sequential]
        public void IndexTest(
            [Values(10, 10, 10, 10, 10)] int pages,
            [Values(null, 10, 12, null, 10, 12)] int? requestedPage,
            [Values(1, 10, 10, 1, 10, 10)] int expectedPage)
        {
            var expectedPageSize = 10;
            var expectedRequestedPage = requestedPage == null ? 1 : requestedPage.Value;

            var videoListPage = new PagedList<Video>(new Video[] { }, expectedPage, expectedPageSize, pages);

            _videoRepositoryMock.Expect(r => r.ListVideos(expectedRequestedPage, expectedPageSize)).Return(videoListPage);


            var result = TestedController.Index(requestedPage);


            VerifyViewResult(result, "List", typeof(PagedList<Video>), videoListPage);
        }
    }
}