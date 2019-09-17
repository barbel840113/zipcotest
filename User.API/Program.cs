using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IntegrationEventDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using UserManagement.API.API.Infrastructure;
using UserManagement.API.API.Infrastructure.DBContext;
namespace UserManagement.API.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string ProgramName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", ProgramName);
                var host = BuildWebHost(configuration, args);

                Log.Information("Applying migrations ({ApplicationContext})...", ProgramName);
                host.MigrateDbContext<UserManagementContext>((context, services) =>              {
                    // Seed Database if apply
                })
                .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

                Log.Information("Starting web host ({ApplicationContext})...", ProgramName);
                host.Run();

                return 0;
            }catch(Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", ProgramName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }         
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddEnvironmentVariables();        

            return builder.Build();
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", ProgramName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .CaptureStartupErrors(false)
               .UseStartup<Startup>()
               .UseApplicationInsights()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseWebRoot("Pics")
               .UseConfiguration(configuration)
               .UseSerilog()
               .Build();
    }
}
