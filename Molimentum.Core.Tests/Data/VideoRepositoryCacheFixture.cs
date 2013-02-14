using System.Web.Caching;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Services;
using NUnit.Framework;
using Rhino.Mocks;
using C=Rhino.Mocks.Constraints;

namespace Molimentum.Tests
{
    [TestFixture]
    public class VideoRepositoryCacheFixture
    {
        [Test]
        public void ListVideosThatAreNotInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var videosStub = mockRepository.Stub<PagedList<Video>>(null, 1, 20, 20);

            var videoRepositoryMock = mockRepository.StrictMock<IVideoRepository>();
            videoRepositoryMock.Expect(repository => repository.ListVideos(1, 20)).Return(videosStub);

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(null);
            cacheServiceMock.Expect(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null))
                .IgnoreArguments()
                .Constraints(C.Is.Anything(), C.Is.Equal(videosStub), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything());

            mockRepository.ReplayAll();

            
            using (var repository = new VideoRepositoryCache(cacheServiceMock, videoRepositoryMock))
            {
                repository.ListVideos(1, 20);
                
                var cacheServiceKey1 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Get(null))[0][0];
                var cacheServiceKey2 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null))[0][0];
            
                Assert.That(cacheServiceKey1, Is.EqualTo(cacheServiceKey2));
            }

            cacheServiceMock.VerifyAllExpectations();
            videoRepositoryMock.VerifyAllExpectations();
        }

        [Test]
        public void ListVideosThatAreInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var videosStub = mockRepository.Stub<PagedList<Video>>(null, 1, 20, 20);

            var videoRepositoryMock = mockRepository.StrictMock<IVideoRepository>();

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(videosStub);

            mockRepository.ReplayAll();
                            
            
            using (var repository = new VideoRepositoryCache(cacheServiceMock, videoRepositoryMock))
            {
                repository.ListVideos(1, 20);
            }

            cacheServiceMock.VerifyAllExpectations();
            videoRepositoryMock.VerifyAllExpectations();
        }

        [Test]
        public void GetVideoThatIsNotInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var videoStub = mockRepository.Stub<Video>();

            var videoRepositoryMock = mockRepository.StrictMock<IVideoRepository>();
            videoRepositoryMock.Expect(repository => repository.GetVideo("video1")).Return(videoStub);

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(null);
            cacheServiceMock.Expect(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null))
                .IgnoreArguments()
                .Constraints(C.Is.Anything(), C.Is.Equal(videoStub), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything(), C.Is.Anything());

            mockRepository.ReplayAll();


            using (var repository = new VideoRepositoryCache(cacheServiceMock, videoRepositoryMock))
            {
                repository.GetVideo("video1");
            
                var cacheServiceKey1 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Get(null))[0][0];
                var cacheServiceKey2 = (string)cacheServiceMock.GetArgumentsForCallsMadeOn(cacheService => cacheService.Add(null, null, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null))[0][0];

                Assert.That(cacheServiceKey1, Is.EqualTo(cacheServiceKey2));
            }

            cacheServiceMock.VerifyAllExpectations();
            videoRepositoryMock.VerifyAllExpectations();
        }
        
        [Test]
        public void GetVideoThatIsInTheCacheTest()
        {
            var mockRepository = new MockRepository();

            var videoStub = mockRepository.Stub<Video>();

            var videoRepositoryMock = mockRepository.StrictMock<IVideoRepository>();

            var cacheServiceMock = mockRepository.StrictMock<ICacheService>();
            cacheServiceMock.Expect(cacheService => cacheService.Get(null))
                .IgnoreArguments()
                .Return(videoStub);

            mockRepository.ReplayAll();

            
            using (var repository = new VideoRepositoryCache(cacheServiceMock, videoRepositoryMock))
            {
                repository.GetVideo("video1");
            }

            cacheServiceMock.VerifyAllExpectations();
            videoRepositoryMock.VerifyAllExpectations();
        }
    }
}