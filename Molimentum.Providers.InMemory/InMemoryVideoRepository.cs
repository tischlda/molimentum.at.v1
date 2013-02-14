using System;
using System.Collections.Generic;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.InMemory
{
    public class InMemoryVideoRepository : IVideoRepository
    {
        private static Dictionary<string, Video> s_videos = new Dictionary<string, Video>();
        
        static InMemoryVideoRepository()
        {
            var thumbnailUri = new Uri("/notfound.jpg", UriKind.Relative);

            var video = new Video
            {
                ID = "1",
                Title = "Video 1",
                Description = "First video",
                EmbedHtml = "<b>Video 1</b>",
                Position = new Position(42, 15),
                ThumbnailWidth = 160,
                ThumbnailHeight = 120,
                ThumbnailUri = thumbnailUri
            };

            s_videos.Add(video.ID, video);
        }

        public Video GetVideo(string videoID)
        {
            return s_videos[videoID];

        }

        public PagedList<Video> ListVideos(int page, int pageSize)
        {
            return new PagedList<Video>(s_videos.Values, 1, 1, 1);
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

        ~InMemoryVideoRepository()
        {
            Dispose(false);
        }
    }
}