using System.Collections.Generic;
using System.Linq;

namespace EveLucrum.Infrastructure
{
    public static class EnumerableExt
    {
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int chunkSize)
        {
            var chunk = new List<T>(chunkSize);
            foreach (var x in source)
            {
                chunk.Add(x);
                if (chunk.Count <= chunkSize)
                {
                    continue;
                }
                yield return chunk;
                chunk = new List<T>(chunkSize);
            }
            if (chunk.Any())
            {
                yield return chunk;
            }
        }
    }
}