using Microsoft.Extensions.Logging;
using PaChangelogAnalyzer.Core.Interfaces;

namespace PaChangelogAnalyzer;

internal class App
{
    private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
    private readonly IProductChangelogService service;
    private readonly IWebScraper webScraper;
    private readonly ILogger<App> logger;

    public App(ILogger<App> logger, Microsoft.Extensions.Configuration.IConfiguration configuration, IProductChangelogService service, IWebScraper webScraper)
    {
        this.configuration = configuration;
        this.service = service;
        this.webScraper = webScraper;
        this.logger = logger;
    }

    public async Task Run()
    {
        logger.LogDebug("Running application...");

        var itemsFromWeb = await webScraper.GetAllProductChangelogItemsFromWeb();

        service.InitializeDb(itemsFromWeb);

        var itemsFromDb = service.GetAllProductChangelogItems();

        //logger.LogDebug("###### Items from Web: {@ItemsFromWeb}", itemsFromWeb);
        logger.LogDebug("###### Items from Db: {@ItemsFromDb}", itemsFromDb);
    }

}