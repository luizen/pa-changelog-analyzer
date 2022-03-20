using Microsoft.Extensions.Logging;
using PaChangelogAnalyzer.Core.Entities;
using PaChangelogAnalyzer.Core.Interfaces;

namespace PaChangelogAnalyzer.Core.Services;

public class ProductChangelogService : IProductChangelogService
{
    private readonly ILogger<ProductChangelogService> logger;
    private readonly IProductChangelogRepository repository;

    public ProductChangelogService(ILogger<ProductChangelogService> logger, IProductChangelogRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public IEnumerable<ProductChangeLogItem> GetAllChangelogItems()
    {
        return repository.GetAllChangelogItems();
    }

    public int InitializeDb()
    {
        return repository.InitializeDb();
    }
}
