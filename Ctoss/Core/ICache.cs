namespace Ctoss.Core;

public interface ICache<in TKey, TEntry>
{
    object? Get(TKey key);
    void Set(TKey key, TEntry entry);
}