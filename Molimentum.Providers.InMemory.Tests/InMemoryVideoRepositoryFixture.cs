using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.InMemory.Tests
{
    [TestFixture]
    public class InMemoryVideoRepositoryFixture : IVideoRepositoryFixture
    {
        protected override IVideoRepository CreateIVideoRepository()
        {
            return new InMemoryVideoRepository();
        }
    }
}