using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,
                                            Action<TContext,IServiceProvider> seeder, 
                                            int? retry = 0) where TContext : DbContext
        {
            int retryValue = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating SQL database with Context {DbContextName}",typeof(TContext).Name);
                    InvokeSeeder(seeder, context, services);
                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while migrating database used on context {DbContextName}", typeof(TContext).Name);
                    while (retryValue > 0)
                    {
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host,seeder, --retryValue);
                    }
                }
                return host;
            }
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, 
                                        TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
