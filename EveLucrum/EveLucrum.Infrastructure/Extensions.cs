using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EveLucrum.Infrastructure
{
    public static class Extensions
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
        
        public static string AbbreviatedFormat(this decimal num)
        {
            if (num >= 10000000000)
                return num.ToString("#,##0,,,B", CultureInfo.InvariantCulture);

            if (num >= 100000000)
                return num.ToString("#,##0,,M", CultureInfo.InvariantCulture);

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }

        public static string AbbreviatedFormat(this long num)
        {
            if (num >= 10000000000)
                return num.ToString("#,##0,,,B", CultureInfo.InvariantCulture);

            if (num >= 100000000)
                return num.ToString("#,##0,,M", CultureInfo.InvariantCulture);

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }
    }
}