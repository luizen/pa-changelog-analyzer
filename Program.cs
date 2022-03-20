using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaChangelogAnalyzer.Config;
using PaChangelogAnalyzer.Core.Interfaces;
using PaChangelogAnalyzer.Core.Services;
using PaChangelogAnalyzer.Infrastructure;
using PaChangelogAnalyzer.Infrastructure.Repositories;
using PaChangelogAnalyzer.Infrastructure.WebScraper;
using Serilog;
using Serilog.Events;

namespace PaChangelogAnalyzer;

public class Program
{
    private static IConfigurationRoot? configuration;

    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var host = BuildHost(args);

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                var app = services.GetRequiredService<App>();
                await app.Run();

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

    private static IHost BuildHost(string[] args)
    {
        Log.Debug("Building host...");

        return new HostBuilder()
            .ConfigureServices(ConfigureServices)
            .UseSerilog()
            .Build();
    }

    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Add logging
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

        // Add options/settings
        serviceCollection.Configure<LiteDbOptions>(configuration.GetSection("LiteDbOptions"));
        serviceCollection.Configure<WebScraperOptions>(configuration.GetSection("WebScraperOptions"));

        // Add app
        serviceCollection.AddTransient<App>();
    }
}