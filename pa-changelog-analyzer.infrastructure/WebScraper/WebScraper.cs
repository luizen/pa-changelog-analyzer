using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaChangelogAnalyzer.Core.ValueObjects;
using PaChangelogAnalyzer.Core.Interfaces;
using AngleSharp;
using PaChangelogAnalyzer.Infrastructure.Options;

namespace PaChangelogAnalyzer.Infrastructure.WebScraper;

public class WebScraper : IWebScraper
{
    private readonly ILogger<WebScraper> logger;
    private readonly IOptions<WebScraperOptions> options;

    public WebScraper(ILogger<WebScraper> logger, IOptions<WebScraperOptions> options)
    {
        logger.LogDebug($"{nameof(WebScraper)} ctor");

        this.logger = logger;
        this.options = options;
    }

    public async Task<IEnumerable<ProductChangeLogItem>> GetAllProductChangelogItemsFromWeb()
    {
        logger.LogDebug(nameof(GetAllProductChangelogItemsFromWeb));
        logger.LogDebug("Options = {@Options}", options);

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        using (var document = await context.OpenAsync(options.Value.Url))
        {
            var pluginNameElements = document.QuerySelectorAll(options.Value.PluginNamesSelector);
            var changeLogElements = document.QuerySelectorAll(options.Value.ChangeLogsSelector);

            var pluginNameItemsCount = pluginNameElements.Length;
            var changeLogItemsCount = changeLogElements.Length;

            if (pluginNameItemsCount != changeLogItemsCount)
            {
                var errorMsg = "The number of plugin name items is different than the number of change log items.";
                var errorMsgTemplate = "{@ErrorMessage}. PluginNameItemsCount = {@PluginNameItemsCount}; ChangeLogItemsCount = {@ChangeLogItemsCount}";
                logger.LogError(errorMsgTemplate, errorMsg, pluginNameItemsCount, changeLogItemsCount);
                throw new InvalidOperationException(errorMsg);
            }

            var result = new List<ProductChangeLogItem>();
            for (int i = 0; i < pluginNameItemsCount; i++)
            {
                var item = new ProductChangeLogItem()
                {
                    Name = pluginNameElements[i].TextContent,
                    Changelog = changeLogElements[i].TextContent //.InnerHtml
                };

                result.Add(item);

                logger.LogDebug("----------------------------------------------");
                logger.LogDebug("Plugin name = {@PluginName}", item.Name);
                logger.LogDebug("Change log = {@ChangeLog}", item.Changelog);
            }

            return result;
        }
    }
}