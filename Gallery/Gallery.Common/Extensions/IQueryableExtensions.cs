using Gallery.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            var lambda = Lambda.ForProperty<TSource, object>(propertyName);
            return source.OrderBy(lambda);
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            var lambda = Lambda.ForProperty<TSource, object>(propertyName);
            return source.OrderByDescending(lambda);
        }
    }
}
