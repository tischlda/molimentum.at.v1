using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    public abstract class ControllerFixtureBase<T> where T : ControllerBase
    {
//        protected static Guid s_testGuid = new Guid("3C78F1F1-9E2B-47DB-83B4-3C7EC37F800C");

        private IPrincipal _principalStub;
        private ControllerContext _controllerContextStub;

        
        [SetUp]
        public void SetUpMocks()
        {
            MockRepository = new MockRepository();

            _principalStub = MockRepository.Stub<IPrincipal>();
            var httpContextBaseStub = MockRepository.Stub<HttpContextBase>();
            httpContextBaseStub.User = _principalStub;

            _controllerContextStub = MockRepository.Stub<ControllerContext>();
            _controllerContextStub.HttpContext = httpContextBaseStub;

            OnSetUpMocks();

            MockRepository.ReplayAll();

            TestedController = CreateTestedController();
            TestedController.ControllerContext = _controllerContextStub;
        }


        [TearDown]
        public void VerifyMocks()
        {
            MockRepository.VerifyAll();
        }

        protected virtual void OnSetUpMocks()
        {

        }


        protected abstract T CreateTestedController();


        protected T TestedController { get; private set; }


        protected MockRepository MockRepository { get; private set; }
        

        protected void AddRole(string role)
        {
            _principalStub.Expect(principal => principal.IsInRole(role)).Return(true).Repeat.Any();
        }


        protected static void VerifyRedirectToRouteResult(ActionResult result, string expectedController = null, string expectedAction = null, string expectedID = null)
        {
            Assert.That(result, Is.TypeOf(typeof(RedirectToRouteResult)), "Result is no RedirectToRouteResult.");

            var redirectToRouteResult = result as RedirectToRouteResult;

            if (expectedController != null) Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo(expectedController), "Redirect to wrong controller.");
            if (expectedAction != null) Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo(expectedAction), "Redirect to wrong action.");
            if (expectedID != null) Assert.That(redirectToRouteResult.RouteValues["id"], Is.EqualTo(expectedID), "Redirect to wrong id.");
        }


        protected static void VerifyViewResult(ActionResult result, string expectedViewName, Type expectedModelType = null, object expectedModel = null)
        {
            Assert.That(result, Is.TypeOf(typeof(ViewResult)), "Result is no ViewResult.");
         
            var viewResult = result as ViewResult;

            Assert.That(viewResult.ViewName, Is.EqualTo(expectedViewName), "Redirected to wrong view.");

            if (expectedModelType != null)
            {
                Assert.That(viewResult.ViewData.Model, Is.Not.Null, "Model not set.");
                Assert.That(expectedModelType.IsAssignableFrom(viewResult.ViewData.Model.GetType()), String.Format("Wrong model type set ('{0}').", viewResult.ViewData.Model.GetType()));
            }
            else
            {
                Assert.That(viewResult.ViewData.Model, Is.Null, "Model set.");
            }

            if (expectedModel != null)
            {
                Assert.That(viewResult.ViewData.Model, Is.EqualTo(expectedModel), String.Format("Wrong model set ('{0}').", expectedModel));
            }
        }


        protected static void VerifyContentResult(ActionResult result, string expectedContentType, string expectedContent)
        {
            Assert.That(result, Is.TypeOf(typeof(ContentResult)), "Result is no ContentResult.");

            var contentResult = result as ContentResult;

            Assert.That(contentResult.ContentType, Is.EqualTo(expectedContentType), "Result is of wrong content type.");
            Assert.That(contentResult.Content, Is.EqualTo(expectedContent), "Result contains wrong content.");
        }
    }
}
