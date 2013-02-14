using System.Linq;
using Molimentum.Repositories;
using NUnit.Framework;

namespace Molimentum.Tests
{
    [TestFixture]
    public abstract class IPictureRepositoryFixture
    {
        [Test]
        public void ListAlbumsTest()
        {
            using (var repository = CreateIPictureRepository())
            {
                var albums = repository.ListAlbums(1, 20);

                Assert.IsNotNull(albums);

                foreach (var album in albums.Items)
                {
                    Assert.That(album.ThumbnailUri, Is.Not.Null);
                }
            }
        }

        [Test]
        public void ListPicturesTest()
        {
            using (var repository = CreateIPictureRepository())
            {
                var album = repository.GetAlbum(repository.ListAlbums(1, 20).Items.First().ID);

                Assert.IsNotNull(album);

                foreach (var picture in album.Pictures)
                {
                    Assert.That(picture.PictureUri, Is.Not.Null);
                    Assert.That(picture.ThumbnailUri, Is.Not.Null);
                }
            }
        }

        protected abstract IPictureRepository CreateIPictureRepository();
    }
}