using System;

namespace Molimentum.Model
{
    public class AlbumSummary : IItem
    {
        public string ID { get; set; }
        
        public Uri ThumbnailUri { get; set;  }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? PositionDateTime { get; set; }
    }
}