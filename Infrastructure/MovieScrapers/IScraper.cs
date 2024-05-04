using Microsoft.Extensions.Logging;

namespace Domain.Abstractions
{
    public interface IScraper
    {
        Task ScrapeActorsAsync();
    }
}
