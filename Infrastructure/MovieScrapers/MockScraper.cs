using Domain.Abstractions;
using Infrastructure.Attributes;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MovieScrapers
{
    [Scraper("MockProvider", "https://www.mock.com")]
    internal class MockScraper : ScraperBase
    {
        public MockScraper(ILogger<MockScraper> logger, IHttpClientFactory httpClientFactory, IActorRepository actorRepository) : base(logger, httpClientFactory, actorRepository)
        {
        }

        public override Task ScrapeActorsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
