using Molimentum.Web.Controllers;
using NUnit.Framework;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class AdministrationControllerFixture : ControllerFixtureBase<AdministrationController>
    {
        protected override AdministrationController CreateTestedController()
        {
            return new AdministrationController();
        }
        
        
        [Test]
        public void WidgetTest()
        {
            var result = TestedController.Widget();

            VerifyViewResult(result, "");
        }
    }
}