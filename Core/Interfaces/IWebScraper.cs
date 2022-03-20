using pa_version_analyzer.Core.Entities;

namespace pa_version_analyzer.Core.Interfaces;

public interface IWebScraper
{
    Task<IEnumerable<ProductChangeLogItem>> GetAllProductChangelogItems();
}
