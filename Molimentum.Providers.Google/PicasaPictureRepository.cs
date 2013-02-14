using System;
using System.IO;
using System.Linq;
using Google.GData.Extensions.Location;
using Google.GData.Photos;
using Molimentum.Model;
using Molimentum.Providers.Google.Configuration;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.Google
{
    public class PicasaPictureRepository : IPictureRepository
    {
        public PagedList<AlbumSummary> ListAlbums(int page, int pageSize)
        {
            var query = new AlbumQuery(PicasaQuery.CreatePicasaUri(PicasaConfiguration.Settings.Gallery.User))
            {
                Thumbsize = PicasaConfiguration.Settings.Pictures.ThumbnailSize.ToString(),
                StartIndex = (page - 1) * pageSize + 1,
                NumberToRetrieve = pageSize
            };

            var feed = GetPicasaFeed(query);
            var items = from PicasaEntry pictureEntry in feed.Entries select PicasaDataMapper.MapPicasaAlbum(pictureEntry);
            items = items.Where(item => item.Title != "Profile Photos" && item.Title != "Scrapbook Photos");

            return new PagedList<AlbumSummary>(items, Utils.CalculatePageNumber(feed.StartIndex, feed.ItemsPerPage), feed.ItemsPerPage, Utils.CalculatePageNumber(feed.TotalResults, feed.ItemsPerPage));
        }

        public Album GetAlbum(string albumID)
        {
            var query = new PhotoQuery(PicasaQuery.CreatePicasaUri(PicasaConfiguration.Settings.Gallery.User, albumID))
            {
                Thumbsize = PicasaConfiguration.Settings.Pictures.ThumbnailSize.ToString(),
                ExtraParameters = PicasaConfiguration.Settings.Pictures.MaximumImageSize.HasValue ?
                                  ("imgmax=" + PicasaConfiguration.Settings.Pictures.MaximumImageSize.Value.ToString()) : ""
            };

            var feed = GetPicasaFeed(query);

            return new Album
            {
                Summary = PicasaDataMapper.MapPicasaAlbum(feed, albumID),
                Pictures = from PicasaEntry pictureEntry in feed.Entries select PicasaDataMapper.MapPicasaImage(pictureEntry)
            };
        }

        public Picture AddPictureToAlbum(string albumID, Stream image, string imageName, string title, string description, DateTime? positionDateTime, Position position)
        {
            var service = new PicasaService(PicasaConfiguration.Settings.Application.Name);

            service.setUserCredentials(PicasaConfiguration.Settings.Authentication.Username, PicasaConfiguration.Settings.Authentication.Password);

            var postUri = new Uri(PicasaQuery.CreatePicasaUri(PicasaConfiguration.Settings.Authentication.Username, albumID));

            var entry = (PicasaEntry)service.Insert(postUri, image, "image/jpeg", imageName);

            entry.Title.Text = title;
            entry.Summary.Text = description;

            if (positionDateTime != null)
            {
                entry.SetPhotoExtensionValue("timestamp", Convert.ToTimestamp(positionDateTime.Value));
            }

            if (position != null)
            {
                entry.Location = new GeoRssWhere();
                entry.Location.Latitude = position.Latitude;
                entry.Location.Longitude = position.Longitude;
            }

            entry = (PicasaEntry)entry.Update();
            
            return PicasaDataMapper.MapPicasaImage(entry);
        }

        private static PicasaFeed GetPicasaFeed(KindQuery query)
        {
            var service = new PicasaService(PicasaConfiguration.Settings.Application.Name);

            service.setUserCredentials(PicasaConfiguration.Settings.Authentication.Username, PicasaConfiguration.Settings.Authentication.Password);

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

        ~PicasaPictureRepository()
        {
            Dispose(false);
        }
    }
}