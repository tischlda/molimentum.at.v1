using System.Linq;
using Molimentum.Repositories;
using NUnit.Framework;

namespace Molimentum.Tests
{
    [TestFixture]
    public abstract class IVideoRepositoryFixture
    {
        [Test]
        public void ListVideosTest()
        {
            using (var repository = CreateIVideoRepository())
            {
                var videos = repository.ListVideos(1, 20);

                Assert.IsNotNull(videos);

                foreach (var video in videos.Items)
                {
                    Assert.That(video.ThumbnailUri, Is.Not.Null);
                }
            }
        }

        [Test]
        public void GetVideoTest()
        {
            using (var repository = CreateIVideoRepository())
            {
                var video = repository.GetVideo(repository.ListVideos(1, 20).Items.First().ID);

                Assert.IsNotNull(video);

                Assert.That(video.EmbedHtml, Is.Not.Null);
            }
        }

        protected abstract IVideoRepository CreateIVideoRepository();
    }
}