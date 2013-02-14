using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class LatestPostsControllerFixture : ControllerFixtureBase<LatestPostsController>
    {
        private IPostRepository _postRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _postRepositoryMock = MockRepository.StrictMock<IPostRepository>();
        }


        protected override LatestPostsController CreateTestedController()
        {
            return new LatestPostsController(_postRepositoryMock);
        }
        
        
        [Test]
        public void WidgetTest()
        {
            var postListPage = new PagedList<Post>(new Post[] { }, 1, 10, 1);

            _postRepositoryMock.Expect(r => r.ListPublished(1, 10)).Return(postListPage);


            var result = TestedController.Widget();


            VerifyViewResult(result, "LatestPostsWidget", typeof(PagedList<Post>), postListPage);
        }
    }
}