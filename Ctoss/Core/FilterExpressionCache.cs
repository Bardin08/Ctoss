using System.Collections.Concurrent;
using Ctoss.Models;

namespace Ctoss.Core;

public class FilterExpressionCache : IFilterExpressionCache
{
    private readonly ConcurrentDictionary<FilterDescriptor, object> _cache = new();

    public object? Get(FilterDescriptor key)
    {
        _ = _cache.TryGetValue(key, out var entry);
        return entry;
    }

    public void Set(FilterDescriptor key, object entry)
    {
        _cache.AddOrUpdate(key, _ => entry, (_, _) => entry);
    }
}