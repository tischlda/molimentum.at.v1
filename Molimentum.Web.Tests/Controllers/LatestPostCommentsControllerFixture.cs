using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class LatestPostCommentsControllerFixture : ControllerFixtureBase<LatestPostCommentsController>
    {
        private IPostCommentRepository _postCommentRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _postCommentRepositoryMock = MockRepository.StrictMock<IPostCommentRepository>();
        }


        protected override LatestPostCommentsController CreateTestedController()
        {
            return new LatestPostCommentsController(_postCommentRepositoryMock);
        }
        
        
        [Test]
        public void WidgetTest()
        {
            var postCommentListPage = new PagedList<PostComment>(new PostComment[] { }, 1, 10, 1);

            _postCommentRepositoryMock.Expect(r => r.ListPublished(1, 10)).Return(postCommentListPage);


            var result = TestedController.Widget();


            VerifyViewResult(result, "LatestPostCommentsWidget", typeof(PagedList<PostComment>), postCommentListPage);
        }
    }
}