using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Converters
{
    public class LocalizableTypeConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            //Use the resource manager to achieve localization.
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
