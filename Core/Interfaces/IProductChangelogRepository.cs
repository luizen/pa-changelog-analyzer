using System.Collections.Generic;
using PaChangelogAnalyzer.Core.Entities;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IProductChangelogRepository
{
    int InitializeDb();

    IEnumerable<ProductChangeLogItem> GetAllChangelogItems();
}
