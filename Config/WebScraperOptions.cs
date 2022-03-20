namespace pa_version_analyzer.Config;

public class WebScraperOptions
{
    public string Url { get; set; } = string.Empty;

    public string PluginNamesSelector { get; set; } = string.Empty;
    
    public string ChangeLogsSelector { get; set; } = string.Empty;    
}
