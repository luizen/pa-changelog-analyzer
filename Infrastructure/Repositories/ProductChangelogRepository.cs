using LiteDB;
using Microsoft.Extensions.Logging;
using pa_version_analyzer.Core.Entities;
using pa_version_analyzer.Core.Interfaces;

namespace pa_version_analyzer.Infrastructure.Repositories;

public class ProductChangelogRepository : IProductChangelogRepository
{
    private readonly LiteDatabase liteDb;
    private readonly ILogger<ProductChangelogRepository> logger;
    private readonly ILiteDbContext dbContext;

    public ProductChangelogRepository(ILogger<ProductChangelogRepository> logger, ILiteDbContext dbContext)
    {
        this.liteDb = dbContext.Database;
        this.logger = logger;
        this.dbContext = dbContext;
    }
    
    public IEnumerable<ProductChangeLogItem> GetAllChangelogItems()
    {
        throw new NotImplementedException();
    }

    public int InitializeDb()
    {
        throw new NotImplementedException();
    }
}
