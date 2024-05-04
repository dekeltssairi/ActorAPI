using Domain.Abstractions;
using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.MovieScrapers
{
    public class MockScraper : IScraper
    {
        public MockScraper(ILogger<MockScraper> logger, IOptions<ScraperConfiguration> options, IHttpClientFactory httpClientFactory, IActorRepository actorRepository)
        {
        }

        public Task ScrapeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
