using System;
using System.IO;
using System.Web.Caching;
using Molimentum.Model;
using Molimentum.Services;

namespace Molimentum.Repositories
{
    public class PictureRepositoryCache : IPictureRepository
    {
        private ICacheService _cacheService;
        private IPictureRepository _underlyingRepository;

        public PictureRepositoryCache(ICacheService cacheService, IPictureRepository underlyingRepository)
        {
            _cacheService = cacheService;
            _underlyingRepository = underlyingRepository;
        }

        public PagedList<AlbumSummary> ListAlbums(int page, int pageSize)
        {
            var cacheKey = "pictureRepository:ListAlbums:" + page + ":" + pageSize;

            var result = (PagedList<AlbumSummary>)_cacheService.Get(cacheKey);
            
            if(result == null)
            {
                result = _underlyingRepository.ListAlbums(page, pageSize);
                _cacheService.Add(cacheKey, result, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return result;
        }

        public Album GetAlbum(string albumID)
        {
            var cacheKey = "pictureRepository:GetAlbum:" + albumID;

            var result = (Album)_cacheService.Get(cacheKey);

            if (result == null)
            {
                result = _underlyingRepository.GetAlbum(albumID);
                _cacheService.Add(cacheKey, result, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return result;
        }

        public Picture AddPictureToAlbum(string albumID, Stream image, string imageName, string title, string description, DateTime? positionDateTime, Position position)
        {
            return _underlyingRepository.AddPictureToAlbum(albumID, image, imageName, title, description, positionDateTime, position);
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

        ~PictureRepositoryCache()
        {
            Dispose(false);
        }
    }
}