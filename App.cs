
using AngleSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using pa_version_analyzer.Core.Interfaces;

internal class App
{
    private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
    private readonly IProductChangelogService service;
    private readonly ILogger<App> logger;

    public App(ILogger<App> logger, Microsoft.Extensions.Configuration.IConfiguration configuration, IProductChangelogService service)
    {
        this.configuration = configuration;
        this.service = service;
        this.logger = logger;
    }

    // public async Task DoWork()
    // {
    //     var config = Configuration.Default.WithDefaultLoader();
    //     var context = BrowsingContext.New(config);
    //     var urls = LoadProductUrlsFromConfig();

    //     foreach (var url in urls)
    //     {
    //         var document = await context.OpenAsync(url);
    //         var selector = "#changelog .section-content";
    //         var sectionContent = document.QuerySelector(selector);
    //         var content = sectionContent?.TextContent;

    //         Console.WriteLine(content);
    //     }
    // }

    public async Task Run()
    {
        logger.LogDebug("App start");

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var url = "https://www.plugin-alliance.com/en/support.html";

        var document = await context.OpenAsync(url);
        var selectorPluginNames = ".ce_changeLogs.block div.toggler u";
        var selectorChangelogs = ".ce_changeLogs.block .accordion .inneraccordion .ce_text.block";

        var pluginNameElements = document.QuerySelectorAll(selectorPluginNames);
        var changeLogElements = document.QuerySelectorAll(selectorChangelogs);
        var pluginNameItemsCount = pluginNameElements.Length;
        var changeLogItemsCount = changeLogElements.Length;

        for (int i = 0; i < pluginNameItemsCount; i++)
        {
            var pluginName = pluginNameElements[i].TextContent;
            var changeLogText = GetChangeLogText(changeLogElements[i]);

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine(pluginName);
            Console.WriteLine(changeLogText);

            Console.WriteLine();
        }

        // foreach (var element in changeLogElements)
        // {
        //     var text = element.TextContent;
        //     var html = element.InnerHtml;
        //     var pluginName = GetPluginName(html);
        //     var changelog = GetChangeLog(html);

        //     Console.WriteLine(pluginName);
        //     Console.WriteLine(changelog);

        //     Console.WriteLine();
        // }
    }

    private string GetChangeLogText(AngleSharp.Dom.IElement element)
    {
        return element.InnerHtml;
    }

    // private string GetChangeLog(string html)
    // {
    //     var brbr = "<br><br>";
    //     var indexBr = html.IndexOf(brbr) + brbr.Length;
    //     var res = html.Substring(indexBr);
    //     return res;
    // }

    // private string GetPluginName(string html)
    // {
    //     var indexBr = html.IndexOf("<br>");
    //     var a1 = html.Substring(0, indexBr);
    //     var a2 = a1.Replace("<p>", "").Replace("Changelog", "").Replace(" - ", "");

    //     return a2;
    // }

    private string[] LoadProductUrlsFromConfig()
    {
        return configuration.GetSection("ProductUrls").Get<string[]>();
    }
}