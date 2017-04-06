
namespace Pharos.Infrastructure.Data.Cache.Interface
{
    public interface ICache<T>
    {
        bool ContainsKey(string key);
        T Get(string key);
        void Remove(string key);
        void Set(string key, T value);
    }
}
