﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Data.SqlClient;

namespace Microsoft.AspNetCore.Hosting
{
    public static class IWebHostDataMigrationExtensions
    {
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {                  

                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                    var retry = Policy.Handle<SqlException>()
                                                 .WaitAndRetry(new TimeSpan[]
                                                 {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(10),
                             TimeSpan.FromSeconds(20),
                                                 });


                    retry.Execute(() => InvokeDbSeeder(seeder, context, services));

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);                
                }
            }

            return webHost;
        }

        private static void InvokeDbSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider serviceProvider)
            where TContext : DbContext
        {           
            context.Database.Migrate();
            seeder.Invoke(context, serviceProvider);
        }
    }


}
