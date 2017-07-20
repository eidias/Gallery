using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Helpers
{
    public class PredicateHelper
    {
        public static Predicate<T> CombineAll<T>(params Predicate<T>[] predicates)
        {
            return target =>
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (!predicate(target))
                    {
                        return false;
                    }
                }
                return true;
            };
        }

        public static Predicate<T> CombineAny<T>(params Predicate<T>[] predicates)
        {
            return target =>
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (predicate(target))
                    {
                        return true;
                    }
                }
                return false;
            };
        }
    }
}
