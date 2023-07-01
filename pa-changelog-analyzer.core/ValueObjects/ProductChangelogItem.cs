namespace PaChangelogAnalyzer.Core.ValueObjects;

public class ProductChangeLogItem
{
    public string Name { get; set; }

    public string Changelog { get; set; }

    public ProductChangeLogItem()
    {
        Name = string.Empty;
        Changelog = string.Empty;
    }

    public ProductChangeLogItem(string name, string changelog)
    {
        Name = name;
        Changelog = changelog;
    }

    public static ProductChangeLogItem FromKeyValuePair(KeyValuePair<string, string> keyValuePair)
    {
        return new ProductChangeLogItem(keyValuePair.Key, keyValuePair.Value);
    }

    public static ProductChangeLogItem FromKeyValue(string key, string value)
    {
        return new ProductChangeLogItem(key, value);
    }
}
