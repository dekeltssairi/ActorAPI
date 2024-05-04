using Domain.Abstractions;
using Infrastructure.Configurations;
using Infrastructure.MovieScrapers;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScraper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ScraperConfiguration>()
                .Bind(configuration.GetSection("ScraperConfiguration"))
                .ValidateDataAnnotations();

            ScraperConfiguration scraperConfig = configuration.GetSection("ScraperConfiguration").Get<ScraperConfiguration>()
                ?? throw new ArgumentNullException("Failed to parse ScraperConfiguration");

            ValidateConfiguration(scraperConfig);

            //Consider to extract to Factory for more flexability instead of register a single Scraper
            switch (scraperConfig.Provider)
            {
                case "IMDb":
                    services.AddScoped<IScraper, IMDbScraper>();
                    break;
                case "MockProvider":
                    services.AddScoped<IScraper, MockScraper>();
                    break;
                default:
                    throw new Exception($"No scraper found for provider: {scraperConfig.Provider}");
            }

            return services;
        }

        private static void ValidateConfiguration(ScraperConfiguration config)
        {
            var validationContext = new ValidationContext(config);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(config, validationContext, validationResults, true))
            {
                var errorMessages = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                throw new ArgumentException($"Invalid ScraperConfiguration: {errorMessages}");
            }
        }
    }
}

