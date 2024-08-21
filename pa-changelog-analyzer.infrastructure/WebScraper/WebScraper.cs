using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaChangelogAnalyzer.Core.ValueObjects;
using PaChangelogAnalyzer.Core.Interfaces;
using AngleSharp;
using PaChangelogAnalyzer.Infrastructure.Options;
using AngleSharp.Html.Dom;

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

        using var allProductsPageDocument = await context.OpenAsync(options.Value.ProductsUrl);

        var pluginLinks = allProductsPageDocument.QuerySelectorAll(options.Value.ProductLinkSelector);

        List<ProductChangeLogItem> result = [];

        foreach (var pluginLink in pluginLinks)
        {
            var link = (IHtmlAnchorElement)pluginLink;

            logger.LogInformation("Product: {@Product}    -- Link: {@Link}", link.Text, link.Href);

            using var pluginPageDocument = await context.OpenAsync(link.Href);
            var changeLogElement = pluginPageDocument.QuerySelector(options.Value.ChangelogSelector);
            var changeLogContent = changeLogElement?.InnerHtml ?? null;

            if (string.IsNullOrWhiteSpace(changeLogContent))
            {
                logger.LogWarning("Changelog element not found for product {@Product}", link.Text);
            }

            result.Add(new ProductChangeLogItem(link.Text, changeLogContent));

            await Task.Delay(100);
        }

        return result;
    }
}