using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            Type type = typeof(TEnum);
            if (!type.IsEnum)
            {
                yield break;
            }
            Array values = Enum.GetValues(type);
            for (int i = 0; i < values.Length; i++)
            {
                yield return (TEnum)values.GetValue(i);
            }
        }
    }
}
