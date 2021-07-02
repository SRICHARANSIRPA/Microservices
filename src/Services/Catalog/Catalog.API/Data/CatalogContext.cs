using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IOptions<ConfigurationValues> configuration)
        {
            var client = new MongoClient(configuration.Value.DatabaseSettings.ConnectionString);
            var database = client.GetDatabase(configuration.Value.DatabaseSettings.DatabaseName);
            Products = database.GetCollection<Product>(configuration.Value.DatabaseSettings.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
