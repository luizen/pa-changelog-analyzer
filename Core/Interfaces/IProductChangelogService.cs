using PaChangelogAnalyzer.Core.Entities;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IProductChangelogService
{
    int InitializeDb(IEnumerable<ProductChangeLogItem> items);

    IEnumerable<ProductChangeLogItem> GetAllProductChangelogItems();

    int CountProductChangelogItems();
}
