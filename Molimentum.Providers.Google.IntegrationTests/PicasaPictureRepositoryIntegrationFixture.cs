using System;
using System.IO;
using Molimentum.Model;
using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.Google.IntegrationTests
{
    [TestFixture]
    [Explicit("Integration")]
    [Category("Integration")]
    public class PicasaPictureRepositoryIntegrationFixture : IPictureRepositoryFixture
    {
        protected override IPictureRepository CreateIPictureRepository()
        {
            return new PicasaPictureRepository();
        }

        [Test]
        [Category("Integration")]
        public void AddPictureTest()
        {
            var repository = CreateIPictureRepository();

            using (var image = File.OpenRead("HPIM3464.JPG"))
            {
                repository.AddPictureToAlbum("5389952507569429265", image, "testImage", "TestImage", "Test",
                                             new DateTime(2009, 01, 01), new Position(42, 15));
            }
        }
    }
}
