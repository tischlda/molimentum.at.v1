using System;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Molimentum.Model;

namespace Molimentum.Services
{
    public interface IFeedService : IDisposable
    {
        SyndicationFeedFormatter CreateFeed(FeedFormat format, UrlHelper urlHelper);
    }
}