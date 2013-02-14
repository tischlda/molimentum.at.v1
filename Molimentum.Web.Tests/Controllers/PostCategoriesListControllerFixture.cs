using System.Collections.Generic;
using Molimentum;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class PostCategoriesListControllerFixture : ControllerFixtureBase<PostCategoriesListController>
    {
        private IPostCategoryRepository _postCategoryRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _postCategoryRepositoryMock = MockRepository.StrictMock<IPostCategoryRepository>();
        }


        protected override PostCategoriesListController CreateTestedController()
        {
            return new PostCategoriesListController(_postCategoryRepositoryMock);
        }
        
        
        [Test]
        public void WidgetTest()
        {
            var postCategories = new PostCategory[] { };
            var postCategoryListPage = new PagedList<PostCategory>(postCategories, 1, 10, 1);

            _postCategoryRepositoryMock.Expect(r => r.List(1, int.MaxValue)).Return(postCategoryListPage);


            var result = TestedController.Widget();


            VerifyViewResult(result, "PostCategoriesListWidget", typeof(IEnumerable<PostCategory>), postCategories);
        }
    }
}