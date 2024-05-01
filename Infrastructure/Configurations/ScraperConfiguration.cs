
namespace Infrastructure.Configurations
{
    public class ScraperConfiguration
    {
        public ScraperConfiguration(string provider)
        {
            Provider = provider;
        }
        public string Provider { get; set; }
    }
}
