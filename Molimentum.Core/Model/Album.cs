using System.Collections.Generic;

namespace Molimentum.Model
{
    public class Album
    {
        public AlbumSummary Summary { get; set; }

        public IEnumerable<Picture> Pictures { get; set; }
    }
}