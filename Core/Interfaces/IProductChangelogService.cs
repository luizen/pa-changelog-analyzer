using PaChangelogAnalyzer.Core.ValueObjects;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IProductChangelogService
{
    int InitializeDb(IEnumerable<ProductChangeLogItem> items);

    IEnumerable<ProductChangeLogItem> GetAllProductChangelogItems();

    int CountProductChangelogItems();
}
