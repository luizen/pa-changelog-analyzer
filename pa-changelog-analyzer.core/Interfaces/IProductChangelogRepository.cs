using PaChangelogAnalyzer.Core.ValueObjects;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IProductChangelogRepository
{
    bool Insert(ProductChangeLogItem item);

    int Insert(IEnumerable<ProductChangeLogItem> items);

    void DeleteAll();

    int Count();

    IEnumerable<ProductChangeLogItem> GetAll();
}
