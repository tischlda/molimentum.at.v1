using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Mvc.Tests
{
    [TestFixture]
    public class IncludeExtensionsTest
    {
        public static HtmlHelper CreateHtmlHelper(ViewDataDictionary viewData)
        {
            var mockViewContext = MockRepository.GenerateStub<ViewContext>(
                MockRepository.GenerateStub<ControllerContext>(),
                MockRepository.GenerateStub<IView>(),
                viewData,
                new TempDataDictionary(),
                new StringWriter());

            var mockViewDataContainer = MockRepository.GenerateStub<IViewDataContainer>();

            mockViewDataContainer.ViewData = viewData;

            return new HtmlHelper(mockViewContext, mockViewDataContainer);
        }


        [Test]
        public void ClientScriptIncludesForPage()
        {
            var htmlHelper = CreateHtmlHelper(new ViewDataDictionary());

            var result = htmlHelper.ClientScriptIncludes(new PageFake());

            Assert.That(result, Is.EqualTo(
                                    "<script type='text/javascript' src='http://foo'></script>\n" +
                                    "<script type='text/javascript' src='http://bar'></script>\n"));
        }

        [Test]
        public void ClientScriptIncludesForMasterPage()
        {
            var htmlHelper = CreateHtmlHelper(new ViewDataDictionary());

            var result = htmlHelper.ClientScriptIncludes(new MasterPageFake());

            Assert.That(result, Is.EqualTo(
                                    "<script type='text/javascript' src='http://masterFoo'></script>\n" +
                                    "<script type='text/javascript' src='http://masterBar'></script>\n" +
                                    "<script type='text/javascript' src='http://foo'></script>\n" +
                                    "<script type='text/javascript' src='http://bar'></script>\n"));
        }

        [Test]
        public void StyleSheetIncludesForPage()
        {
            var htmlHelper = CreateHtmlHelper(new ViewDataDictionary());

            var result = htmlHelper.StyleSheetIncludes(new PageFake());

            Assert.That(result, Is.EqualTo(
                                    "<link rel='stylesheet' type='text/css' href='/Styles/foo.css'/>\n" +
                                    "<link rel='stylesheet' type='text/css' href='/Styles/bar.css'/>\n"));
        }

        [Test]
        public void StyleSheetIncludesForMasterPage()
        {
            var htmlHelper = CreateHtmlHelper(new ViewDataDictionary());

            var result = htmlHelper.StyleSheetIncludes(new MasterPageFake());

            Assert.That(result, Is.EqualTo(
                                    "<link rel='stylesheet' type='text/css' href='/Styles/masterFoo.css'/>\n" +
                                    "<link rel='stylesheet' type='text/css' href='/Styles/masterBar.css'/>\n" +
                                    "<link rel='stylesheet' type='text/css' href='/Styles/foo.css'/>\n" +
                                    "<link rel='stylesheet' type='text/css' href='/Styles/bar.css'/>\n"));
        }

        [ClientScriptInclude("http://foo", "http://bar")]
        [StyleSheetInclude("/Styles/foo.css", "/Styles/bar.css")]
        class PageFake : Page
        {
        }

        [ClientScriptInclude("http://masterFoo", "http://masterBar", "http://foo")]
        [StyleSheetInclude("/Styles/masterFoo.css", "/Styles/masterBar.css", "/Styles/foo.css")]
        class MasterPageFake : MasterPage
        {
            private readonly PageFake _pageFake = new PageFake();

            public override Page Page
            {
                get
                {
                    return _pageFake;
                }
            }
        }
    }
}