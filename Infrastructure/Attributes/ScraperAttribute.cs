using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Attributes
{
    public class ScraperAttribute : Attribute
    {
        public string ProviderName { get; }

        public ScraperAttribute(string providerName)
        {
            ProviderName = providerName;
        }
    }
}
