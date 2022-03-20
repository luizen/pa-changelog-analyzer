using LiteDB;

namespace pa_version_analyzer.Infrastructure;

public interface ILiteDbContext
{
    LiteDatabase Database { get; }
}
