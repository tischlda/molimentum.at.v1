using System;

namespace Molimentum.Model
{
    public class Picture : IPosition
    {
        public string ID { get; set; }
        
        public Uri PictureUri { get; set; }

        public uint? PictureWidth { get; set; }

        public uint? PictureHeight { get; set; }

        public Uri ThumbnailUri { get; set; }

        public uint? ThumbnailWidth { get; set; }

        public uint? ThumbnailHeight { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        
        public DateTime? PositionDateTime { get; set; }

        public Position Position { get; set; }
    }
}