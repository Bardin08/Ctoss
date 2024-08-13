namespace Ctoss.Core;

public interface ICache<in TKey, TEntry>
{
    Task<object>? Get(TKey key);
    Task Set(TKey key, TEntry entry);
}