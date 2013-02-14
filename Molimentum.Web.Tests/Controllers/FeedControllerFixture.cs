using System.ServiceModel.Syndication;
using System.Xml;
using Molimentum.Model;
using Molimentum.Services;
using Molimentum.Web.Controllers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Molimentum.Web.Tests.Controllers
{
    [TestFixture]
    public class FeedControllerFixture : ControllerFixtureBase<FeedsController>
    {
        private IFeedService _feedServiceMock;
        private SyndicationFeedFormatter _syndicationFeedFormaterMock;


        protected override void OnSetUpMocks()
        {
            base.OnSetUpMocks();

            _feedServiceMock = MockRepository.StrictMock<IFeedService>();
            _syndicationFeedFormaterMock = MockRepository.StrictMock<SyndicationFeedFormatter>();
        }


        protected override FeedsController CreateTestedController()
        {
            return new FeedsController(_feedServiceMock);
        }


        [Test, Sequential]
        public void FeedTest(
            [Values(FeedFormat.Atom, FeedFormat.Rss)] FeedFormat format)
        {
            _feedServiceMock.Expect(m => m.CreateFeed(format, TestedController.Url)).Return(_syndicationFeedFormaterMock);
            _syndicationFeedFormaterMock.Expect(m => m.WriteTo(null)).IgnoreArguments()
                .WhenCalled(c => ((XmlWriter)c.Arguments[0]).WriteRaw("<feed/>"));

            
            var result = TestedController.GetFeed(format);

            
            VerifyContentResult(result, "text/xml", "<feed/>");
        }
    }
}