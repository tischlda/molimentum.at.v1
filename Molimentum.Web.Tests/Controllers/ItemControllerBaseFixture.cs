using Molimentum.Web.Controllers;
using NUnit.Framework;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class ItemControllerBaseFixture
    {
        [Test]
        public void ItemControllerWithDefaultPageSizeTest()
        {
            var controller = new ItemControllerWithDefaultPageSize();

            Assert.That(controller.DefaultPageSize, Is.EqualTo(1));
        }

        [DefaultPageSize(1)]
        private class ItemControllerWithDefaultPageSize : ItemControllerBase
        {

        }

        
        [Test]
        public void SecondLevelItemControllerWithDefaultPageSizeTest()
        {
            var controller = new SecondLevelItemControllerWithDefaultPageSize();

            Assert.That(controller.DefaultPageSize, Is.EqualTo(2));
        }

        [DefaultPageSize(2)]
        private class SecondLevelItemControllerWithDefaultPageSize : ItemControllerWithDefaultPageSize
        {

        }


        [Test]
        public void ItemControllerWithoutDefaultPageSizeTest()
        {
            var controller = new ItemControllerWithoutDefaultPageSize();

            Assert.That(controller.DefaultPageSize, Is.EqualTo(10));
        }

        private class ItemControllerWithoutDefaultPageSize : ItemControllerBase
        {

        }
    }
}