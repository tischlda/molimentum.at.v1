using System;
using System.IO;
using System.Web.Caching;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Services;
using NUnit.Framework;
using Rhino.Mocks;
using C = Rhino.Mocks.Constraints;

namespace Molimentum.Tests
{
    [TestFixture]
    public class PictureRepositoryCacheFixture
    {
        [Test]
        public void ListAlbumsThatAreNotInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var albumsStub = mockRepository.Stub<PagedList<AlbumSummary>>(null, 1, 20, 20);

            var pictureRepositoryMock = mockRepository.StrictMock<IPictureRepository>();
            pictureRepositoryMock.Expect(repository => repository.ListAlbums(1, 20)).Return(albumsStub);

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(null);
            cacheServiceMock.Expect(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null))
                .IgnoreArguments()
                .Constraints(C.Is.Anything(), C.Is.Equal(albumsStub), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything());

            mockRepository.ReplayAll();

            
            using (var repository = new PictureRepositoryCache(cacheServiceMock, pictureRepositoryMock))
            {
                repository.ListAlbums(1, 20);
                
                var cacheServiceKey1 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Get(null))[0][0];
                var cacheServiceKey2 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null))[0][0];
            
                Assert.That(cacheServiceKey1, Is.EqualTo(cacheServiceKey2));
            }

            cacheServiceMock.VerifyAllExpectations();
            pictureRepositoryMock.VerifyAllExpectations();
        }

        [Test]
        public void ListAlbumsThatAreInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var albumsStub = mockRepository.Stub<PagedList<AlbumSummary>>(null, 1, 20, 20);

            var pictureRepositoryMock = mockRepository.StrictMock<IPictureRepository>();

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(albumsStub);

            mockRepository.ReplayAll();
                            
            
            using (var repository = new PictureRepositoryCache(cacheServiceMock, pictureRepositoryMock))
            {
                repository.ListAlbums(1, 20);
            }

            cacheServiceMock.VerifyAllExpectations();
            pictureRepositoryMock.VerifyAllExpectations();
        }

        [Test]
        public void GetAlbumThatIsNotInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var albumStub = mockRepository.Stub<Album>();

            var pictureRepositoryMock = mockRepository.StrictMock<IPictureRepository>();
            pictureRepositoryMock.Expect(repository => repository.GetAlbum("album1")).Return(albumStub);

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(null);
            cacheServiceMock.Expect(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null))
                .IgnoreArguments()
                .Constraints(C.Is.Anything(), C.Is.Equal(albumStub), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything());

            mockRepository.ReplayAll();


            using (var repository = new PictureRepositoryCache(cacheServiceMock, pictureRepositoryMock))
            {
                repository.GetAlbum("album1");
            
                var cacheServiceKey1 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Get(null))[0][0];
                var cacheServiceKey2 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null))[0][0];

                Assert.That(cacheServiceKey1, Is.EqualTo(cacheServiceKey2));
            }

            cacheServiceMock.VerifyAllExpectations();
            pictureRepositoryMock.VerifyAllExpectations();
        }
        
        [Test]
        public void GetAlbumThatIsInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var albumStub = mockRepository.Stub<Album>();

            var pictureRepositoryMock = mockRepository.StrictMock<IPictureRepository>();

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(albumStub);

            mockRepository.ReplayAll();

            
            using (var repository = new PictureRepositoryCache(cacheServiceMock, pictureRepositoryMock))
            {
                repository.GetAlbum("album1");
            }

            cacheServiceMock.VerifyAllExpectations();
            pictureRepositoryMock.VerifyAllExpectations();
        }

        [Test]
        public void AddPictureToAlbumTest()
        {
            var mockRepository = new MockRepository();
            var picture = new Picture();

            var albumID = "testAlbumID";
            var image = mockRepository.Stub<Stream>();
            var imageName = "testImageName";
            var title = "testTitle";
            var description = "testDescription";
            var positionDateTime = new DateTime(2001, 1, 1);
            var position = new Position(42, 15);

            var pictureRepositoryMock = mockRepository.StrictMock<IPictureRepository>();
            pictureRepositoryMock
                .Expect(m => m.AddPictureToAlbum(albumID, image, imageName, title, description, positionDateTime, position))
                .Return(picture);

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();

            mockRepository.ReplayAll();


            using (var repository = new PictureRepositoryCache(cacheServiceMock, pictureRepositoryMock))
            {
                var result = repository.AddPictureToAlbum(albumID, image, imageName, title, description, positionDateTime, position);
                Assert.That(result, Is.EqualTo(picture));
            }

            cacheServiceMock.VerifyAllExpectations();
            pictureRepositoryMock.VerifyAllExpectations();
        }
    }
}