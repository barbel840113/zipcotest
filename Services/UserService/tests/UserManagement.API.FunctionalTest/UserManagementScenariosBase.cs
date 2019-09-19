using IntegrationEventDB;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UserManagement.API.API;
using UserManagement.API.API.Infrastructure.DBContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.API.FunctionalTest
{
    public class UserManagementScenariosBase
    {
        public TestServer CreateTestServer()
        {
            var path = Assembly.GetAssembly(typeof(UserManagementScenariosBase))
              .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                 .AddEnvironmentVariables();
                }).UseStartup<Startup>();

            var dbContextOptionBuilder = new DbContextOptionsBuilder<UserManagementContext>().UseInMemoryDatabase(databaseName: "temp");          

            var testServer = new TestServer(hostBuilder);            

            testServer.Host.MigrateDbContext<UserManagementContext>((context, services) =>
                {

                })
                .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

            return testServer;
        }
    }
}
