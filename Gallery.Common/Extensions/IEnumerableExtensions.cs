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

        public static IEnumerable<int> FindGaps(this IEnumerable<int> collection)
        {
            //Test with new[] { 1, 2, 3, 6, 7, 17, 23, 44, 56, 57, 58 }
            return Enumerable.Range(1, collection.Max() + 1).Except(collection);
        }

    }
}
