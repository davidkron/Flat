using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat
{

    public static class Extensions
    {
        public static ISet<T> ToSet<T>(this IEnumerable<T> enumerable) =>
            enumerable.ToImmutableHashSet();
        public static HashSet<T> ToMutableSet<T>(this IEnumerable<T> enumerable) =>
            new HashSet<T>(enumerable);
    }
}
