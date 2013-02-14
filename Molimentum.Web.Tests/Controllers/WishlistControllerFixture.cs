using Molimentum.Web.Controllers;
using NUnit.Framework;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class WishlistControllerFixture : ControllerFixtureBase<WishlistController>
    {
        protected override WishlistController CreateTestedController()
        {
            return new WishlistController();
        }
        
        
        [Test]
        public void WidgetTest()
        {
            var result = TestedController.Widget();

            VerifyViewResult(result, "WishlistWidget");
        }
    }
}