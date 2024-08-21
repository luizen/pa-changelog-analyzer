namespace PaChangelogAnalyzer.Infrastructure.Options;
public class WebScraperOptions
{
    public string ProductsUrl { get; set; } = string.Empty;

    public string ProductLinkSelector { get; set; } = string.Empty;

    public string ChangelogSelector { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string PluginNamesSelector { get; set; } = string.Empty;

    public string ChangeLogsSelector { get; set; } = string.Empty;
}
