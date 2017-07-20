using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Extensions
{
    public static class StringExtensions
    {
        public static object ConvertTo(this String s, Type type)
        {
            //Since value types cannot be created from empty strings we will return null and allow the datasource to convert to the default value of the type.
            if (type.IsValueType && s.Equals(String.Empty))
            {
                return null;
            }
            //GetConverter returns the defined converter for the supplied type. E.g. DateTime will return an instance of DateTimeConverter.
            TypeConverter typeConverter = TypeDescriptor.GetConverter(type);

            //The converter will return either the current or the default value of the supplied refrence type.
            return typeConverter.ConvertFromString(s);
        }

        public static Type ResolveType(this String s)
        {
            if (s != null)
            {
                //Question mark is used to identify nullable value types in a string.
                if (s.EndsWith("?"))
                {
                    //But it cannot be used to resolve a nullable type directly.
                    return typeof(Nullable<>).DefineGenericArgument(AppDomain.CurrentDomain.GetNamedType(s.TrimEnd('?')));
                }
                else
                {
                    //We the extension method because Type.GetType(string) only searches loaded libraries.
                    return AppDomain.CurrentDomain.GetNamedType(s);
                }
            }
            return default(Type);
        }
    }
}
