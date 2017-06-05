using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static SortedList<TKey, TElement> ToSortedList<TSource, TKey, TElement>(this IEnumerable<TSource> collection, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            SortedList<TKey, TElement> sortedList = new SortedList<TKey, TElement>();
            foreach (var item in collection)
            {
                sortedList.Add(keySelector(item), elementSelector(item));
            }
            return sortedList;
        }
    }
}
