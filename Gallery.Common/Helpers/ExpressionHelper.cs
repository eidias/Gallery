using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Helpers
{
    public static class Lambda
    {
        public static Expression<Func<TSource, TProperty>> ForProperty<TSource, TProperty>(string propertyName)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TSource));
            MemberExpression memberExpression = Expression.Property(parameterExpression, propertyName);
            UnaryExpression expressionBody = Expression.Convert(memberExpression, typeof(TProperty));
            return Expression.Lambda<Func<TSource, TProperty>>(expressionBody, parameterExpression);
        }
    }
}
