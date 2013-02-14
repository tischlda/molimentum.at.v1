using System;

namespace Molimentum.Model
{
    public class Video : IPosition, IItem
    {
        public string ID { get; set; }
        
        public string EmbedHtml { get; set; }

        public Uri ThumbnailUri { get; set; }

        public uint? ThumbnailWidth { get; set; }

        public uint? ThumbnailHeight { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        
        public DateTime? PositionDateTime { get; set; }

        public Position Position { get; set; }
    }
}