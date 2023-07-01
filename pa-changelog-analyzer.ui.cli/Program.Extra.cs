using PaChangelogAnalyzer.Core.Interfaces;
using PaChangelogAnalyzer.Core.Services;
using PaChangelogAnalyzer.Infrastructure;
using PaChangelogAnalyzer.Infrastructure.Options;
using PaChangelogAnalyzer.Infrastructure.Repositories;
using PaChangelogAnalyzer.Infrastructure.WebScraper;
using PaChangelogAnalyzer.Ui.Cli.Options;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace PaChangelogAnalyzer.Ui.Cli;

public partial class Program
{
    private static void SetupLogging(ParserResult<object>? parserResult)
    {
        Log.Debug("Setting up logging settings...");

        SetLoggingLevelFromArgs(parserResult);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(levelSwitch)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .CreateLogger();
    }

    private static void SetLoggingLevelFromArgs(ParserResult<object>? parserResult)
    {
        Log.Debug("Setting logging level from args...");

        LoggingLevels level = LoggingLevels.Debug;
        var commonResult = parserResult.WithParsed<CommonOptions>(x => level = x.LoggingLevel);
        int levelAsNumber = ((int)level);

        levelSwitch.MinimumLevel = (LogEventLevel)levelAsNumber;
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
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
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