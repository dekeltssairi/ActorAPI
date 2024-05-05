using Domain.Abstractions;

namespace API.HostedServices
{
    public class ScraperHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ScraperHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scraper = scope.ServiceProvider.GetRequiredService<IScraper>();
            await scraper.ScrapeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
