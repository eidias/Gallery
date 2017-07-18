using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            Type type = enumValue.GetType();
            string name = enumValue.ToString();
            FieldInfo fieldInfo = type.GetField(name);
            return fieldInfo.GetCustomAttribute<TAttribute>();
        }

        public static string ToLocalizedString(this Enum e)
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(e);
            return typeConverter.ConvertTo(e, typeof(string)) as string;
        }
    }
}
