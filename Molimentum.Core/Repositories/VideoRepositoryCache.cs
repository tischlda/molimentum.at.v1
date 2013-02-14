using System;
using System.Web.Caching;
using Molimentum.Model;
using Molimentum.Services;

namespace Molimentum.Repositories
{
    public class VideoRepositoryCache : IVideoRepository
    {
        private ICacheService _cacheService;
        private IVideoRepository _underlyingRepository;

        public Video GetVideo(string videoID)
        {
            var cacheKey = "videoRepository:GetVideo:" + videoID;

            var result = (Video)_cacheService.Get(cacheKey);

            if (result == null)
            {
                result = _underlyingRepository.GetVideo(videoID);
                _cacheService.Add(cacheKey, result, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return result;
        }

        public PagedList<Video> ListVideos(int page, int pageSize)
        {
            var cacheKey = "videoRepository:ListVideos:" + page + ":" + pageSize;

            var result = (PagedList<Video>)_cacheService.Get(cacheKey);

            if (result == null)
            {
                result = _underlyingRepository.ListVideos(page, pageSize);
                _cacheService.Add(cacheKey, result, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return result;
        }

        public VideoRepositoryCache(ICacheService cacheService, IVideoRepository underlyingRepository)
        {
            _cacheService = cacheService;
            _underlyingRepository = underlyingRepository;
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
                _cacheService = null;
                _underlyingRepository = null;
            }

            _disposed = true;
        }

        ~VideoRepositoryCache()
        {
            Dispose(false);
        }
    }
}