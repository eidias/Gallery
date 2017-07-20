using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

        public static DataTable AsDataTable<T>(this IEnumerable<T> items)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            DataTable dataTable = new DataTable();
            foreach (var property in properties)
            {
                //DataTables do not support nullable types.
                var propertyType = property.PropertyType.GetUnderlyingType();
                dataTable.Columns.Add(property.Name, propertyType);
            }
            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dataTable = new DataTable();
            var firstItem = collection.FirstOrDefault();
            if (firstItem != null)
            {
                PropertyInfo[] propertyInfos = firstItem.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    dataTable.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }
                foreach (T item in collection)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow.BeginEdit();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        dataRow[propertyInfo.Name] = propertyInfo.GetValue(item, null) ?? DBNull.Value;
                    }
                    dataRow.EndEdit();
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }


    }
}
