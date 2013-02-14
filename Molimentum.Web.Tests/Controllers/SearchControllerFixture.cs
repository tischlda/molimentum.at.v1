using System.Collections.Generic;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class SearchControllerFixture : ControllerFixtureBase<SearchController>
    {
        private IPostRepository _postRepositoryMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _postRepositoryMock = MockRepository.StrictMock<IPostRepository>();
        }


        protected override SearchController CreateTestedController()
        {
            return new SearchController(_postRepositoryMock);
        }


        [Test]
        public void SearchTest()
        {
            var result = TestedController.Search();

            VerifyViewResult(result, "");
        }


        [Test]
        public void WidgetTest()
        {
            var tags = new TagSummary[] { };

            _postRepositoryMock.Expect(r => r.ListTags()).Return(tags);


            var result = TestedController.Widget();


            VerifyViewResult(result, "SearchWidget", typeof(IEnumerable<TagSummary>), tags);
        }
    }
}