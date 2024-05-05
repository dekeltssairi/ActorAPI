using Domain.Abstractions;
using Infrastructure.Attributes;
using Infrastructure.Configurations;
using Infrastructure.MovieScrapers;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScraper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ScraperConfiguration>()
                .Bind(configuration.GetSection("ScraperConfiguration"))
                .ValidateDataAnnotations();

            ScraperConfiguration config = configuration.GetSection("ScraperConfiguration").Get<ScraperConfiguration>()
                ?? throw new ArgumentNullException("Failed to parse ScraperConfiguration");


            var scraperAssembly = Assembly.Load("Infrastructure");

            var scraperType = scraperAssembly.GetTypes()
                .First(t => typeof(IScraper).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract &&
                                     t.GetCustomAttribute<ScraperAttribute>()?.ProviderName == config.Provider);

            ScraperAttribute? scraperAttribute = scraperType.GetCustomAttribute<ScraperAttribute>();


            if (scraperType != null)
            {
                services.AddScoped(typeof(IScraper), scraperType);
            }
            else
            {
                throw new KeyNotFoundException($"No scraper found for provider: {config.Provider}");
            }

            return services;
        }
    }
}

