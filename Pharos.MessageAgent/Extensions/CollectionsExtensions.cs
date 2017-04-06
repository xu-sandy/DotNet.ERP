using System.Collections.Generic;

namespace Pharos.MessageAgent.Extensions
{
    public static class CollectionsExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> ranges)
        {
            foreach (var item in ranges)
            {
                list.Add(item);
            }
        }
    }
}
