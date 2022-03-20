using LiteDB;
using Microsoft.Extensions.Options;
using pa_version_analyzer.Config;

namespace pa_version_analyzer.Infrastructure;

public class LiteDbContext : ILiteDbContext
{
    public LiteDatabase Database { get; }

    public LiteDbContext(IOptions<LiteDbOptions> options)
    {
        Database = new LiteDatabase(options.Value.DatabaseLocation);
    }
}
