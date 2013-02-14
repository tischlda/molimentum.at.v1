using System;
using Google.GData.Photos;
using Molimentum.Model;

namespace Molimentum.Providers.Google
{
    static class PicasaDataMapper
    {
        public static AlbumSummary MapPicasaAlbum(PicasaEntry albumEntry)
        {
            if (!albumEntry.IsAlbum) throw new ArgumentException("Entry is not an album.", "albumEntry");

            var album = new AlbumAccessor(albumEntry);

            return new AlbumSummary
                       {
                           ID = album.Id,
                           ThumbnailUri = new Uri(albumEntry.Media.Thumbnails[0].Url),
                           Title = album.AlbumTitle,
                           Description = album.AlbumSummary,
                           PositionDateTime = albumEntry.Published
                       };
        }

        public static AlbumSummary MapPicasaAlbum(PicasaFeed feed, string albumID)
        {
            return new AlbumSummary
            {
                ID = albumID,
                ThumbnailUri = null,
                Title = feed.Title.Text,
                Description = feed.Subtitle.Text,
                PositionDateTime = feed.Updated
            };
        }

        public static Picture MapPicasaImage(PicasaEntry photoEntry)
        {
            if (!photoEntry.IsPhoto) throw new ArgumentException("Entry is not a photo.", "photoEntry");

            return new Picture
                       {
                           ID = photoEntry.Id.AbsoluteUri,
                           PictureUri = new Uri(photoEntry.Media.Content.Attributes["url"].ToString()),
                           PictureWidth = Convert.ToNullableUInt32(photoEntry.Media.Content.Width),
                           PictureHeight = Convert.ToNullableUInt32(photoEntry.Media.Content.Height),
                           ThumbnailUri = new Uri(photoEntry.Media.Thumbnails[0].Url),
                           ThumbnailWidth = Convert.ToNullableUInt32(photoEntry.Media.Thumbnails[0].Width),
                           ThumbnailHeight = Convert.ToNullableUInt32(photoEntry.Media.Thumbnails[0].Height),
                           Title = photoEntry.Title.Text,
                           Description = photoEntry.Media.Description.Value,
                           Position = photoEntry.Location == null ? null : new Position(photoEntry.Location.Latitude, photoEntry.Location.Longitude),
                           PositionDateTime = Convert.FromTimestamp(photoEntry.GetPhotoExtensionValue("timestamp"))
                       };
        }
    }
}