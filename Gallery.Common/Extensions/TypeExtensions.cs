using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static Type GetUnderlyingType(this Type type)
        {
            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }
            return type;
        }

        public static Type DefineGenericArgument(this Type type, Type definedType)
        {
            var genericArguments = type.GetGenericArguments();
            genericArguments[0] = definedType;
            return type.MakeGenericType(genericArguments);
        }

        public static IEnumerable GetValidationAttributes(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName).GetCustomAttributes(typeof(ValidationAttribute), false);
        }
    }
}
