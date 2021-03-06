using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,int? retry = 0)
        {
            int retryValue = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrating postresql database..");
                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    //Deleting the Existing Table
                    command.CommandText = "DROP TABLE IF EXISTS COUPON";
                    command.ExecuteNonQuery();

                    //Creating the Table
                    command.CommandText = @"CREATE TABLE COUPON(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();

                    //seeding the table
                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postresql database..");

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An Error occured while migrating the postresql database");
                    if(retryValue<50)
                    {
                        retryValue++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryValue);
                    }
                }

                return host;
            }
        }
    }
}
