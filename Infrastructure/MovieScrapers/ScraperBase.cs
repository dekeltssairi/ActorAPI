using Microsoft.Extensions.Logging;

namespace Domain.Abstractions
{
    public abstract class ScraperBase
    {
        protected readonly ILogger _logger;
        protected readonly HttpClient _httpClient;
        protected readonly IActorRepository _actorRepository;

        public ScraperBase(ILogger logger, IHttpClientFactory httpClientFactory, IActorRepository actorRepository)
        {
            _logger = logger;
            _actorRepository = actorRepository;
            _httpClient = httpClientFactory.CreateClient(this.GetType().Name);
        }
        public abstract Task ScrapeActorsAsync();
    }
}
