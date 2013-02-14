using Molimentum.Web.Controllers;
using NUnit.Framework;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class TranslateControllerFixture : ControllerFixtureBase<TranslateController>
    {
        protected override TranslateController CreateTestedController()
        {
            return new TranslateController();
        }
        
        
        [Test]
        public void WidgetTest()
        {
            var result = TestedController.Widget();

            VerifyViewResult(result, "TranslateWidget");
        }
    }
}