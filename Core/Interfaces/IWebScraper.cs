using System.Collections.Generic;
using System.Threading.Tasks;
using PaChangelogAnalyzer.Core.Entities;

namespace PaChangelogAnalyzer.Core.Interfaces;

public interface IWebScraper
{
    Task<IEnumerable<ProductChangeLogItem>> GetAllProductChangelogItems();
}
