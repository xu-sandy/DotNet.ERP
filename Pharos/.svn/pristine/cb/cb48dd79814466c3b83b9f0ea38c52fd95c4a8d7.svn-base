using StackExchange.Redis;

namespace Pharos.MessageAgent.Extensions
{
    public static class RedisExtensions
    {
        public static ConnectionMultiplexer GetConnection(this string connectionString)
        {
            return ConnectionMultiplexer.Connect(connectionString);
        }
    }
}
