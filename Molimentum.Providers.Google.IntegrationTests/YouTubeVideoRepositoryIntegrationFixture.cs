using Molimentum.Repositories;
using Molimentum.Tests;
using NUnit.Framework;

namespace Molimentum.Providers.Google.IntegrationTests
{
    [TestFixture]
    [Explicit("Integration")]
    [Category("Integration")]
    public class YouTubeVideoRepositoryIntegrationFixture : IVideoRepositoryFixture
    {
        protected override IVideoRepository CreateIVideoRepository()
        {
            return new YouTubeVideoRepository();
        }
    }
}