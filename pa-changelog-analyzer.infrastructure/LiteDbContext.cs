using LiteDB;
using Microsoft.Extensions.Options;
using PaChangelogAnalyzer.Infrastructure.Options;

namespace PaChangelogAnalyzer.Infrastructure;

public class LiteDbContext : ILiteDbContext
{
    public LiteDatabase Database { get; }

    public LiteDbContext(IOptions<LiteDbOptions> options)
    {
        Database = new LiteDatabase(options.Value.DatabaseLocation);
    }
}
