using System.Collections.Immutable;
using PaChangelogAnalyzer.Core.ValueObjects;

namespace PaChangelogAnalyzer.Core.Extensions;

public static class ProductChangeLogItemExtensions
{
    public static IDictionary<string, string> ToDictionary(this IEnumerable<ProductChangeLogItem> instance)
    {
        if (instance == null)
            return ImmutableDictionary<string, string>.Empty;

        return instance.ToDictionary(key => key.Name, value => value.Changelog);
    }
}
