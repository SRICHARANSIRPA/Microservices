using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Configuration
{
    public class ConfigurationValues
    {
        public CacheSettings CacheSettings;
    }

    public class CacheSettings
    {
        public string ConnectionString { get; set; }
    }
}
