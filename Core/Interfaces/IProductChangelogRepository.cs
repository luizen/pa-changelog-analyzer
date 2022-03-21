using PaChangelogAnalyzer.Core.Entities;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IProductChangelogRepository
{
    bool Insert(ProductChangeLogItem item);

    int Insert(IEnumerable<ProductChangeLogItem> items);

    void DeleteAll();

    int Count();

    Dictionary<string, string> GetAllAsDictionary();

    IEnumerable<ProductChangeLogItem> GetAll();
}
