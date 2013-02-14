using System;
using System.Collections.Generic;
using System.IO;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum;

namespace Molimentum.Providers.InMemory
{
    public class InMemoryPictureRepository : IPictureRepository
    {
        private static List<AlbumSummary> s_albumSummaries = new List<AlbumSummary>();
        private static Dictionary<string, Album> s_albums = new Dictionary<string, Album>();

        static InMemoryPictureRepository()
        {
            var pictureUri = new Uri("/notfound.jpg", UriKind.Relative);
            var thumbnailUri = new Uri("/notfound.jpg", UriKind.Relative);

            var albumSummary = new AlbumSummary
            {
                ID = "1",
                Title = "Album 1",
                Description = "first album",
                PositionDateTime = new DateTime(2000, 1, 1),
                ThumbnailUri = thumbnailUri
            };
            
            s_albumSummaries.Add(albumSummary);


            var picture = new Picture
            {
                ID = "1",
                Title = "Picture 1",
                Description = "First picture",
                Position = new Position(42, 15),
                PositionDateTime = new DateTime(2000, 1, 1),
                PictureWidth = 640,
                PictureHeight = 480,
                PictureUri = pictureUri,
                ThumbnailWidth = 160,
                ThumbnailHeight = 120,
                ThumbnailUri = thumbnailUri
            };


            var album = new Album
            {
                Summary = albumSummary,
                Pictures = new List<Picture> { picture }
            };

            s_albums.Add(album.Summary.ID, album);
        }

        public PagedList<AlbumSummary> ListAlbums(int page, int pageSize)
        {
            return new PagedList<AlbumSummary>(s_albumSummaries, 1, 1, 1);
        }

        public Album GetAlbum(string albumID)
        {
            return s_albums[albumID];
        }

        public Picture AddPictureToAlbum(string albumID, Stream image, string imageName, string title, string description, DateTime? positionDateTime, Position position)
        {
            throw new NotImplementedException();
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

        ~InMemoryPictureRepository()
        {
            Dispose(false);
        }
    }
}