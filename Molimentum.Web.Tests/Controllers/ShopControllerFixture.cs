using Molimentum.Web.Controllers;
using NUnit.Framework;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class ShopControllerFixture : ControllerFixtureBase<ShopController>
    {
        protected override ShopController CreateTestedController()
        {
            return new ShopController();
        }
        
        
        [Test]
        public void IndexTest()
        {
            var result = TestedController.Index();

            VerifyViewResult(result, "");
        }
    }
}