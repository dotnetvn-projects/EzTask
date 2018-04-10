using System;
using System.Collections.Generic;
using System.Text;

namespace EzTask.Framework.Common
{
    public static class Collections
    {
        public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int size)
        {
            for (int i = 0; i < locations.Count; i += size)
            {
                yield return locations.GetRange(i, Math.Min(size, locations.Count - i));
            }
        }
    }
}
