using System;
using System.Collections.Generic;
using LiteDB;
using Microsoft.Extensions.Logging;
using PaChangelogAnalyzer.Core.Entities;
using PaChangelogAnalyzer.Core.Interfaces;

namespace PaChangelogAnalyzer.Infrastructure.Repositories;

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
