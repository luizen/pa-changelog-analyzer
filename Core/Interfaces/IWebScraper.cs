using PaChangelogAnalyzer.Core.ValueObjects;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IWebScraper
{
    Task<IEnumerable<ProductChangeLogItem>> GetAllProductChangelogItemsFromWeb();
}
