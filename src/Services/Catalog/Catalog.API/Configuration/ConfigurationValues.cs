namespace Catalog.API
{
    public class ConfigurationValues
    {
        public DatabaseSettings DatabaseSettings { get; set; }
    }
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}