namespace PaChangelogAnalyzer.Core.ValueObjects;

public class ProductChangeLogItem
{
    public string Name { get; set; } = string.Empty;

    public string? Changelog { get; set; }

    public bool HasChangelog
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Changelog);
        }
    }

    public ProductChangeLogItem()
    {
        Name = string.Empty;
        Changelog = null;
    }

    public ProductChangeLogItem(string name, string? changelog)
    {
        Name = name;
        Changelog = changelog;
    }

    public static ProductChangeLogItem FromKeyValuePair(KeyValuePair<string, string?> keyValuePair)
    {
        return new ProductChangeLogItem(keyValuePair.Key, keyValuePair.Value);
    }

    public static ProductChangeLogItem FromKeyValue(string key, string? value)
    {
        return new ProductChangeLogItem(key, value);
    }
}
