using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.InMemory.Tests
{
    [TestFixture]
    public class InMemoryPictureRepositoryFixture : IPictureRepositoryFixture
    {
        protected override IPictureRepository CreateIPictureRepository()
        {
            return new InMemoryPictureRepository();
        }
    }
}
