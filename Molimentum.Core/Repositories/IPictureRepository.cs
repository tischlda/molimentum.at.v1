using System;
using System.IO;
using Molimentum.Model;

namespace Molimentum.Repositories
{
    public interface IPictureRepository : IDisposable
    {
        PagedList<AlbumSummary> ListAlbums(int page, int pageSize);

        Album GetAlbum(string albumID);

        Picture AddPictureToAlbum(string albumID, Stream image, string imageName, string title, string description, DateTime? positionDateTime, Position position);
    }
}