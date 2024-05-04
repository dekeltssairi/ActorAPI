
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Configurations
{
    public class ScraperConfiguration
    {
        public ScraperConfiguration() {}

        [Required]
        public string Provider { get; set; }
        
        [Required]
        public string Uri { get; set; }
    }
}
