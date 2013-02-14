using System;
using System.Linq;
using Google.GData.YouTube;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Providers.Google.Configuration;

namespace Molimentum.Providers.Google
{
    public class YouTubeVideoRepository : IVideoRepository
    {
        public Video GetVideo(string videoID)
        {
            var query = new YouTubeQuery(YouTubeQuery.CreateUserUri(YouTubeConfiguration.Settings.Gallery.User))
            {
                Query = videoID
            };

            var feed = GetYouTubeFeed(query);

            return YouTubeDataMapper.MapYouTubeVideo((YouTubeEntry)feed.Entries.First());
        }

        public PagedList<Video> ListVideos(int page, int pageSize)
        {
            var query = new YouTubeQuery(YouTubeQuery.CreateUserUri(YouTubeConfiguration.Settings.Gallery.User))
            {
                StartIndex = (page - 1) * pageSize + 1,
                NumberToRetrieve = pageSize
            };

            var feed = GetYouTubeFeed(query);
            var items = from YouTubeEntry videoEntry in feed.Entries select YouTubeDataMapper.MapYouTubeVideo(videoEntry);

            return new PagedList<Video>(items, Utils.CalculatePageNumber(feed.StartIndex, feed.ItemsPerPage), feed.ItemsPerPage, Utils.CalculatePageNumber(feed.TotalResults, feed.ItemsPerPage));
        }

        private static YouTubeFeed GetYouTubeFeed(YouTubeQuery query)
        {
            var service = new YouTubeService(YouTubeConfiguration.Settings.Application.Name);

            service.setUserCredentials(YouTubeConfiguration.Settings.Authentication.Username, YouTubeConfiguration.Settings.Authentication.Password);

            return service.Query(query);
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
            }

            _disposed = true;
        }

        ~YouTubeVideoRepository()
        {
            Dispose(false);
        }
    }
}