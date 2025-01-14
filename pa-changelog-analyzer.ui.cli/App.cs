// using System.Net.Http.Json;
// using System.Text;
using System.Text.Json;
// using ConsoleTables;
// using PaChangelogAnalyzer.Core.Extensions;
using PaChangelogAnalyzer.Core.Interfaces;
using PaChangelogAnalyzer.Core.ValueObjects;
// using PaChangelogAnalyzer.Ui.Cli.Options;

namespace PaChangelogAnalyzer.Ui.Cli;

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

    public async Task Run(ParserResult<object> parserResult)
    {
        logger.LogInformation("Running application...");

        logger.LogInformation("Loading changelog from PA website...");
        var itemsFromWeb = await webScraper.GetAllProductChangelogItemsFromWeb();

        await WriteToJson(itemsFromWeb, "2024");
        await WriteToJson(itemsFromWeb, "2025");

        return;


        // bool dbWasJustInitialized = false;
        // parserResult.WithParsed<InitDbOptions>(_ => {
        //     logger.LogInformation("Initializing database...");
        //     service.InitializeDb(itemsFromWeb);
        //     dbWasJustInitialized = true;
        // });

        // if (dbWasJustInitialized)
        //     return;

        // logger.LogInformation("Veryfing if newest PA changelog is different from the previously database...");

        // var itemsFromDb = service.GetAllProductChangelogItems();
        // var itemsFromDbDic = itemsFromDb.ToDictionary();
        // var itemsFromWebDic = itemsFromWeb.ToDictionary();
        // var differentItems = new List<ComparisonInfo>();

        // logger.LogDebug("###### Items from Web: {@ItemsFromWeb}", itemsFromWeb);
        // logger.LogDebug("###### Items from Db: {@ItemsFromDb}", itemsFromDb);

        // foreach (var itemFromWeb in itemsFromWebDic)
        // {
        //     var changeLogFromWeb = itemFromWeb.Value;
        //     var changeLogFromDb = itemsFromDbDic.ContainsKey(itemFromWeb.Key) ? itemsFromDbDic[itemFromWeb.Key] : string.Empty;

        //     if (string.Compare(changeLogFromWeb, changeLogFromDb, StringComparison.InvariantCultureIgnoreCase) != 0)
        //     {
        //         var compInfo = new ComparisonInfo
        //         (
        //              ProductChangeLogItem.FromKeyValue(itemFromWeb.Key, changeLogFromWeb),
        //              ProductChangeLogItem.FromKeyValue(itemFromWeb.Key, changeLogFromDb)
        //         );

        //         differentItems.Add(compInfo);
        //     }
        // }

        // if (!differentItems.Any())
        // {
        //     logger.LogInformation("You are up to date. No new changes found in the PA website");
        //     return;
        // }

        // logger.LogInformation("###### Updates available! There are {@DifferentItemsCount} changes you are not aware of.", differentItems.Count);
        // logger.LogInformation("###### Changes:");

        // foreach (var item in differentItems)
        //     logger.LogInformation("Plugin name: {@Plugin}. Comparison results: {@ComparisonResults}", item.FromWeb.Name, item);        
    }

    private async Task WriteToJson(IEnumerable<ProductChangeLogItem> itemsFromWeb, string year)
    {
        var items = itemsFromWeb.Where(item => item.HasChangelog && item.Changelog!.Contains(year)).ToList();

        if (items.Count == 0)
        {
            logger.LogInformation($"No changelog items containing '{year}' found.");
            return;
        }

        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        logger.LogInformation($"Writing output to file {year}...");
        var filePath = $"output-file-plugins-pa-{year}.json";
        await File.WriteAllTextAsync(filePath, json);
    }
}