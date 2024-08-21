using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaChangelogAnalyzer.Core.ValueObjects;
using PaChangelogAnalyzer.Core.Interfaces;
using AngleSharp;
using PaChangelogAnalyzer.Infrastructure.Options;
using AngleSharp.Html.Dom;
using System.Collections.Concurrent;
using System.Diagnostics;

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
        List<ProductChangeLogItem> result = [];

        logger.LogDebug(nameof(GetAllProductChangelogItemsFromWeb));
        logger.LogDebug("Options = {@Options}", options);

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        using var allProductsPageDocument = await context.OpenAsync(options.Value.ProductsUrl);
        var pluginLinks = allProductsPageDocument.QuerySelectorAll(options.Value.ProductLinkSelector);

        var pluginPagesToScrape = new ConcurrentBag<PluginLink>();

        foreach (var pluginLink in pluginLinks)
        {
            var link = (IHtmlAnchorElement)pluginLink;
            pluginPagesToScrape.Add(new PluginLink() { Text = link.Text, Href = link.Href });
        }

        logger.LogDebug("Before Parallel.ForEach");
        var tasks = new List<Task>();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (var currentPluginLink in pluginPagesToScrape)
        {
            tasks.Add(Task.Run(async () =>
            {
                // var context = BrowsingContext.New(config);

                // Load the page contents
                using var pluginPageDocument = await context.OpenAsync(currentPluginLink.Href);

                // Try to find the changelog element
                var changeLogElement = pluginPageDocument.QuerySelector(options.Value.ChangelogSelector);
                var changeLogContent = changeLogElement?.InnerHtml ?? null;

                if (string.IsNullOrWhiteSpace(changeLogContent))
                {
                    logger.LogWarning("Changelog element not found for product {@Product}", currentPluginLink.Text);
                }
                else
                {
                    logger.LogInformation("Changelog found for the Product: {@Product}", currentPluginLink.Text);
                }

                result.Add(new ProductChangeLogItem(currentPluginLink.Text, changeLogContent));
            }));
        }

        await Task.WhenAll(tasks);

        stopwatch.Stop();
        logger.LogDebug("Elapsed time: {Elapsed}", stopwatch.Elapsed);

        return result;
    }
}