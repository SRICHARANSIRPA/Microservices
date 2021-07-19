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
        public CatalogContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionString");
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseName"));
            Products = database.GetCollection<Product>(configuration.GetValue<string>("CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
