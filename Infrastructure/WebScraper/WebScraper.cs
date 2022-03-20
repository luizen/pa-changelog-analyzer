using AngleSharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using pa_version_analyzer.Config;
using pa_version_analyzer.Core.Entities;
using pa_version_analyzer.Core.Interfaces;

namespace pa_version_analyzer.Infrastructure.WebScraper;

public class WebScraper : IWebScraper
{
    private readonly ILogger<WebScraper> logger;
    private readonly IOptions<WebScraperOptions> options;

    public WebScraper(ILogger<WebScraper> logger, IOptions<WebScraperOptions> options)
    {
        this.logger = logger;
        this.options = options;
    }

    public async Task<IEnumerable<ProductChangelogItem>> GetAllProductChangelogItems()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var url = options.Value.Url;
        var result = new List<ProductChangelogItem>();

        var document = await context.OpenAsync(url);
        var selector = options.Value.Selector;
        var elements = document.QuerySelectorAll(selector);

        foreach (var element in elements)
        {
            var text = element.TextContent;
            var html = element.InnerHtml;
            var pluginName = GetPluginName(html);
            var changelog = GetChangeLog(html);

            result.Add(new ProductChangelogItem()
            {
                Name = pluginName,
                Changelog = changelog
            });

            // yield return new ProductChangelogItem()
            // {
            //     Name = pluginName,
            //     Changelog = changelog
            // };
        }

        return result;
    }

    private string GetChangeLog(string html)
    {
        var brbr = "<br><br>";
        var indexBr = html.IndexOf(brbr) + brbr.Length;
        var res = html.Substring(indexBr);
        return res;
    }

    private string GetPluginName(string html)
    {
        var indexBr = html.IndexOf("<br>");
        var a1 = html.Substring(0, indexBr);
        var a2 = a1.Replace("<p>", "").Replace("Changelog", "").Replace(" - ", "");

        return a2;
    }

}
