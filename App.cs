using Microsoft.Extensions.Logging;
using PaChangelogAnalyzer.Core.Extensions;
using PaChangelogAnalyzer.Core.Interfaces;
using PaChangelogAnalyzer.Core.ValueObjects;

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
        
        var itemsFromDbDic = itemsFromDb.ToDictionary();
        var itemsFromWebDic = itemsFromWeb.ToDictionary();
        
        var differentItems = new List<ComparisonInfo>();

        foreach (var itemFromWeb in itemsFromWebDic)
        {
            var changeLogFromWeb = itemFromWeb.Value;
            var changeLogFromDb = itemsFromDbDic.ContainsKey(itemFromWeb.Key) ? itemsFromDbDic[itemFromWeb.Key] : string.Empty;

            if (string.Compare(changeLogFromWeb, changeLogFromDb, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                var compInfo = new ComparisonInfo
                (
                     ProductChangeLogItem.FromKeyValue(itemFromWeb.Key, changeLogFromWeb),
                     ProductChangeLogItem.FromKeyValue(itemFromWeb.Key, changeLogFromDb)
                );

                differentItems.Add(compInfo);
            }                
        }
        
        logger.LogDebug("###### Items from Web: {@ItemsFromWeb}", itemsFromWeb);
        logger.LogDebug("###### Items from Db: {@ItemsFromDb}", itemsFromDb);
    }

}