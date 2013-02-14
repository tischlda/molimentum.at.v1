using System;
using G = Google.GData.YouTube;
using Molimentum.Model;
using Molimentum.Providers.Google.Configuration;

namespace Molimentum.Providers.Google
{
    static class YouTubeDataMapper
    {
        public static Video MapYouTubeVideo(G.YouTubeEntry videoEntry)
        {
            return new Video
                       {
                           ID = videoEntry.VideoId,
                           ThumbnailUri = new Uri(videoEntry.Media.Thumbnails[0].Url),
                           EmbedHtml = String.Format(YouTubeConfiguration.Settings.Embedding.HtmlTemplate, videoEntry.VideoId),
                           Title = videoEntry.Title.Text,
                           Description = videoEntry.Media.Description.Value,
                           PositionDateTime = videoEntry.Published,
                           Position = videoEntry.Location == null ? null : new Position(videoEntry.Location.Latitude, videoEntry.Location.Longitude)
                       };
        }
    }
}