using Microsoft.Extensions.Logging;
using pa_version_analyzer.Core.Entities;
using pa_version_analyzer.Core.Interfaces;

namespace pa_version_analyzer.Core.Services;

public class ProductChangelogService : IProductChangelogService
{
    private readonly ILogger<ProductChangelogService> logger;
    private readonly IProductChangelogRepository repository;

    public ProductChangelogService(ILogger<ProductChangelogService> logger, IProductChangelogRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public IEnumerable<ProductChangelogItem> GetAllChangelogItems()
    {
        return repository.GetAllChangelogItems();
    }

    public int InitializeDb()
    {
        return repository.InitializeDb();
    }
}
