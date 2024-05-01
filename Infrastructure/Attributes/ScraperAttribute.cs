using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScraperAttribute : Attribute
    {
        public string ProviderName { get; }
        public Uri BaseUri { get; }

        public ScraperAttribute(string providerName, string baseUri)
        {
            ProviderName = providerName;
            BaseUri = new Uri(baseUri);
        }
    }
}
