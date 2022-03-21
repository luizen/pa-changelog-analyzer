using PaChangelogAnalyzer.Core.Entities;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IProductChangelogService
{
    int InitializeDb(IEnumerable<ProductChangeLogItem> items);

    IEnumerable<ProductChangeLogItem> GetAllProductChangelogItems();

    Dictionary<string, string> GetAllProductChangelogItemsAsDictionary();

    int CountProductChangelogItems();
}
