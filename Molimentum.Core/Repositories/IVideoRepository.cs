using System;
using Molimentum.Model;

namespace Molimentum.Repositories
{
    public interface IVideoRepository : IDisposable
    {
        Video GetVideo(string videoID);

        PagedList<Video> ListVideos(int page, int pageSize);
    }
}