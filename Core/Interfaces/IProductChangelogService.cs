using pa_version_analyzer.Core.Entities;

namespace pa_version_analyzer.Core.Interfaces;

public interface IProductChangelogService
{
    int InitializeDb();

    IEnumerable<ProductChangelogItem> GetAllChangelogItems();
}
