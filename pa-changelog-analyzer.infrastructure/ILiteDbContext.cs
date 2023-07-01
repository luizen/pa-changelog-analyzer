using LiteDB;

namespace PaChangelogAnalyzer.Infrastructure;

public interface ILiteDbContext
{
    LiteDatabase Database { get; }
}
