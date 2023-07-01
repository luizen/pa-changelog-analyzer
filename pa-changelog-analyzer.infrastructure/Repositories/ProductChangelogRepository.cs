using LiteDB;
using Microsoft.Extensions.Logging;
using PaChangelogAnalyzer.Core.ValueObjects;
using PaChangelogAnalyzer.Core.Interfaces;

namespace PaChangelogAnalyzer.Infrastructure.Repositories;

public class ProductChangelogRepository : IProductChangelogRepository
{
    private readonly LiteDatabase liteDb;
    private readonly ILogger<ProductChangelogRepository> logger;
    private readonly ILiteDbContext dbContext;

    public ProductChangelogRepository(ILogger<ProductChangelogRepository> logger, ILiteDbContext dbContext)
    {
        logger.LogDebug($"{nameof(ProductChangelogRepository)} ctor");

        this.liteDb = dbContext.Database;
        this.logger = logger;
        this.dbContext = dbContext;
    }

    public int Count()
    {
        logger.LogDebug(nameof(Count));
        return liteDb.GetCollection<ProductChangeLogItem>().Count();
    }

    public void DeleteAll()
    {
        logger.LogDebug(nameof(DeleteAll));

        var deletedCount = liteDb.GetCollection<ProductChangeLogItem>().DeleteAll();
        logger.LogDebug("Deleted {@DeletedCount} items", deletedCount);
    }

    public IEnumerable<ProductChangeLogItem> GetAll()
    {
        logger.LogDebug(nameof(GetAll));

        return liteDb.GetCollection<ProductChangeLogItem>().FindAll();
    }

    public bool Insert(ProductChangeLogItem item)
    {
        logger.LogDebug(nameof(Insert));

        var col = liteDb.GetCollection<ProductChangeLogItem>();
        var res = col.Insert(item);

        // Create an index over the Name property (if it doesn't exist)
        col.EnsureIndex(x => x.Name);

        logger.LogDebug("Insert result {@Result}", res);

        return res;
    }

    public int Insert(IEnumerable<ProductChangeLogItem> items)
    {
        logger.LogDebug($"{nameof(Insert)} multiple");

        var col = liteDb.GetCollection<ProductChangeLogItem>();
        var res = col.InsertBulk(items);

        // Create an index over the Name property (if it doesn't exist)
        col.EnsureIndex(x => x.Name);

        logger.LogDebug("Inserted {@InsertedItemsCount} items", res);

        return res;
    }
}
