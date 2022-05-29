using System;
using System.IO;
using System.Threading.Tasks;
using Catalog.API.Data;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.UnitTests.System.Repositories
{
    public class DbFixture : IDisposable
    {
        public DbFixture()
        {
            var configuration = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json")
                       .Build();

            connString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            dbName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");

            this.DbContext = new CatalogContext(configuration);
        }

        string connString = string.Empty;
        string dbName = string.Empty;
        
        public CatalogContext DbContext { get; }

        public void Dispose()
        {
            var client = new MongoClient(connString);
            client.DropDatabase(dbName);
        }
    }
}