using Domain.Abstractions;
using Infrastructure.Attributes;
using Infrastructure.Configurations;
using System.Reflection;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScraper(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("ScraperConfiguration").Get<ScraperConfiguration>()
                ?? throw new ArgumentNullException("Failed to parse ScraperConfiguration");

            var scraperAssembly = Assembly.Load("Infrastructure");

            var scraperType = scraperAssembly.GetTypes()
                .First(t => typeof(ScraperBase).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract &&
                                     t.GetCustomAttribute<ScraperAttribute>()?.ProviderName.Equals(config.Provider, StringComparison.OrdinalIgnoreCase) == true);

            ScraperAttribute? scraperAttribute = scraperType.GetCustomAttribute<ScraperAttribute>();

            services.AddHttpClient(scraperType.Name, client =>
            {
                client.BaseAddress = scraperAttribute!.BaseUri;
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-" + scraperType.Name);
            });

            if (scraperType != null)
            {
                services.AddScoped(typeof(ScraperBase), scraperType);
            }
            else
            {
                throw new KeyNotFoundException($"No scraper found for provider: {config.Provider}");
            }

            return services;
        }
    }
}

