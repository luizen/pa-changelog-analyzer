using Microsoft.Extensions.Logging;
using PaChangelogAnalyzer.Core.ValueObjects;
using PaChangelogAnalyzer.Core.Interfaces;

namespace PaChangelogAnalyzer.Core.Services;

public class ProductChangelogService : IProductChangelogService
{
    private readonly ILogger<ProductChangelogService> logger;
    private readonly IProductChangelogRepository repository;

    public ProductChangelogService(ILogger<ProductChangelogService> logger, IProductChangelogRepository repository)
    {
        logger.LogDebug($"{nameof(ProductChangelogService)} ctor");
        this.logger = logger;
        this.repository = repository;
    }

    public int CountProductChangelogItems()
    {
        logger.LogDebug(nameof(CountProductChangelogItems));
        return repository.Count();
    }

    public IEnumerable<ProductChangeLogItem> GetAllProductChangelogItems()
    {
        logger.LogDebug(nameof(GetAllProductChangelogItems));
        return repository.GetAll();
    }

    public int InitializeDb(IEnumerable<ProductChangeLogItem> items)
    {
        logger.LogDebug(nameof(InitializeDb));

        repository.DeleteAll();
        return repository.Insert(items);
    }
}
