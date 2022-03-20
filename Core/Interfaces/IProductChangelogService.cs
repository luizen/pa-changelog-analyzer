using PaChangelogAnalyzer.Core.Entities;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IProductChangelogService
{
    int InitializeDb();

    IEnumerable<ProductChangeLogItem> GetAllChangelogItems();
}
