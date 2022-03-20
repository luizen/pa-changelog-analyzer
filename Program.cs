using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using pa_version_analyzer.Config;
using pa_version_analyzer.Core.Interfaces;
using pa_version_analyzer.Core.Services;
using pa_version_analyzer.Infrastructure;
using pa_version_analyzer.Infrastructure.Repositories;
using pa_version_analyzer.Infrastructure.WebScraper;
using Serilog;
using Serilog.Events;

// // Setup Host
// var host = CreateDefaultBuilder().Build();

// // Invoke Worker
// using IServiceScope serviceScope = host.Services.CreateScope();
// IServiceProvider provider = serviceScope.ServiceProvider;

// var workerInstance = provider.GetRequiredService<App>();
// await workerInstance.DoWork2();

// host.Run();

// // Console.Read();


// static IHostBuilder CreateDefaultBuilder()
// {
//     return Host.CreateDefaultBuilder()
//         .ConfigureAppConfiguration(app =>
//         {
//             app.AddJsonFile("appsettings.json");
//         })
//         .ConfigureServices(services =>
//         {
//             services.AddSingleton<App>();
//         });
// }

class Program
{
    private static IConfigurationRoot? configuration;

    private static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        Log.Debug("Building host");
        var host = BuildHost(args);

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                // Log.Debug("Parsing arguments");
                // var arguments = ParseArguments(args);

                Log.Debug("Starting application");
                var app = services.GetRequiredService<App>();
                // await app.Run(arguments);
                await app.Run();

                Log.Debug("Returning 0");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An error occured. Returning code 1");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }

    public static IHost BuildHost(string[] args)
    {
        return new HostBuilder()
            .ConfigureServices(ConfigureServices)
            .UseSerilog()
            .Build();
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddLogging();

        // Build configuration
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Add access to generic IConfigurationRoot
        serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

        // Add DbContext
        serviceCollection.AddSingleton<ILiteDbContext, LiteDbContext>();

        // Add services
        serviceCollection
            .AddTransient<IProductChangelogService, ProductChangelogService>()
            .AddTransient<IProductChangelogRepository, ProductChangelogRepository>()
            .AddTransient<IWebScraper, WebScraper>();

        serviceCollection.Configure<LiteDbOptions>(configuration.GetSection("LiteDbOptions"));
        serviceCollection.Configure<WebScraperOptions>(configuration.GetSection("WebScraperOptions"));

        // Add app
        serviceCollection.AddTransient<App>();
    }
}