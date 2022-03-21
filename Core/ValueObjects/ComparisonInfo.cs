namespace PaChangelogAnalyzer.Core.ValueObjects;

public class ComparisonInfo
{
    private readonly ProductChangeLogItem fromWeb;
    private readonly ProductChangeLogItem fromDb;

    public ProductChangeLogItem FromWeb => fromWeb;
    public ProductChangeLogItem FromDb => fromDb;

    public ComparisonInfo(ProductChangeLogItem fromWeb, ProductChangeLogItem fromDb)
    {
        this.fromWeb = fromWeb;
        this.fromDb = fromDb;
    }

    public ComparisonInfo(KeyValuePair<string, string> fromWeb, KeyValuePair<string, string> fromDb)
    {
        this.fromWeb = ProductChangeLogItem.FromKeyValuePair(fromWeb);
        this.fromDb = ProductChangeLogItem.FromKeyValuePair(fromDb);
    }
}
