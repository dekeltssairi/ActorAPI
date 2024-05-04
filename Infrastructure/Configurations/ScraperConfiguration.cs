
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Configurations
{
    public enum ScraperProvider
    {
        IMDb,
        MockProvider,
    }
    public class ScraperConfiguration
    {
        public ScraperConfiguration() {}

        [Required]
        public ScraperProvider Provider { get; set; }
        
        [Required]
        public string Uri { get; set; }
    }
}
