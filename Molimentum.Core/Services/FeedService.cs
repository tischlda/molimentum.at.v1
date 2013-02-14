using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Web.Security;
using Molimentum.Model;
using Molimentum.Repositories;

namespace Molimentum.Services
{
    public class FeedService : IFeedService
    {
        private IPostRepository _postRepository;
        private IPictureRepository _pictureRepository;
        private IVideoRepository _videoRepository;
        private IUrlResolverService _urlResolverService;
        private const string c_videoTemplate = "<p>{0}</p>\n{1}";
        private const string c_albumTemplate = "<p>{0}</p>\n<a href=\"{1}\"><img src=\"{2}\" /></a>";

        public FeedService(IPostRepository postRepository, IPictureRepository pictureRepository, IVideoRepository videoRepository, IUrlResolverService urlResolverService)
        {
            _postRepository = postRepository;
            _pictureRepository = pictureRepository;
            _videoRepository = videoRepository;
            _urlResolverService = urlResolverService;
        }

        public SyndicationFeedFormatter CreateFeed(FeedFormat format, UrlHelper urlHelper)
        {
            var items =
                    ListPictures(urlHelper)
                .Union(
                    ListPosts(urlHelper))
                .OrderByDescending(item => item.PublishDate);

            var feed = new SyndicationFeed("Molimentum", "lat.: Bemühung, große Anstrengung", new Uri("http://molimentum.at")) { Items = items };

            return GetFormatter(format, feed);
        }

        private IEnumerable<SyndicationItem> ListPosts(UrlHelper urlHelper)
        {
            var authors = new Dictionary<Guid, SyndicationPerson>();

            foreach (var post in _postRepository.ListPublished(1, 10).Items)
            {
                Trace.Assert(post.IsPublished, "Unpublished post in feed.");

                var item = new SyndicationItem(post.Title, post.Body, null);
                var permaLink = _urlResolverService.CreateDetailLink(urlHelper, post, true);
                item.AddPermalink(permaLink);
                item.Id = permaLink.ToString();
                item.Categories.Add(new SyndicationCategory("Posts"));

                foreach (var tag in post.Tags) item.Categories.Add(new SyndicationCategory(tag));

                if (authors.ContainsKey(post.AuthorID))
                {
                    item.Authors.Add(authors[post.AuthorID]);
                }
                else
                {
                    var user = Membership.GetUser(post.AuthorID);

                    if (user != null)
                    {
                        var author = new SyndicationPerson(null, user.UserName, null);

                        authors.Add(post.AuthorID, author);
                        item.Authors.Add(author);
                    }
                }

                item.PublishDate = post.PublishDate.Value;
                item.LastUpdatedTime = post.PositionDateTime.Value;

                if (post.Position != null)
                {
                    item.ElementExtensions.Add(
                        "point",
                        "http://www.georss.org/georss",
                        String.Format(NumberFormatInfo.InvariantInfo, "{0} {1}", post.Position.Latitude, post.Position.Longitude));
                }

                yield return item;
            }
        }

        private IEnumerable<SyndicationItem> ListVideos(UrlHelper urlHelper)
        {
            foreach (var video in _videoRepository.ListVideos(1, 10).Items)
            {
                var permaLink = _urlResolverService.CreateDetailLink(urlHelper, video, true);

                var item = new SyndicationItem("Video: " + video.Title, String.Format(c_videoTemplate, video.Description, video.EmbedHtml), null)
                {
                    PublishDate = video.PositionDateTime.Value,
                    LastUpdatedTime = video.PositionDateTime.Value,
                    Id = permaLink.ToString()
                };

                item.Categories.Add(new SyndicationCategory("Videos"));

                item.AddPermalink(permaLink);

                yield return item;
            }
        }

        private IEnumerable<SyndicationItem> ListPictures(UrlHelper urlHelper)
        {
            foreach (var album in _pictureRepository.ListAlbums(1, 10).Items)
            {
                var permaLink = _urlResolverService.CreateDetailLink(urlHelper, album, true);

                var item = new SyndicationItem("Fotos: " + album.Title, String.Format(c_albumTemplate, album.Description, permaLink, album.ThumbnailUri), null)
                {
                    PublishDate = album.PositionDateTime.Value,
                    LastUpdatedTime = album.PositionDateTime.Value,
                    Id = permaLink.ToString()
                };

                item.Categories.Add(new SyndicationCategory("Fotos"));

                item.AddPermalink(permaLink);

                yield return item;
            }
        }

        private static SyndicationFeedFormatter GetFormatter(FeedFormat format, SyndicationFeed feed)
        {
            switch (format)
            {
                case FeedFormat.Atom:
                    return new Atom10FeedFormatter(feed);

                case FeedFormat.Rss:
                    return new Rss20FeedFormatter(feed);

                default:
                    throw new Exception(String.Format("Unknown FeedFormat '{0}'.", feed));
            }
        }


        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // release managed resources
                }

                // release unmanaged resources
                // disposal of injected dependencies is managed by the IOC container
                _postRepository = null;
                _pictureRepository = null;
                _videoRepository = null;
                _urlResolverService = null;
            }

            _disposed = true;
        }

        ~FeedService()
        {
            Dispose(false);
        }
    }
}