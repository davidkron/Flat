using System.Collections.Generic;
using System.Collections.Immutable;

namespace Flat
{

    internal static class Extensions
    {
        public static ISet<T> ToSet<T>(this IEnumerable<T> enumerable) =>
            enumerable.ToImmutableHashSet();
        public static HashSet<T> ToMutableSet<T>(this IEnumerable<T> enumerable) =>
            new HashSet<T>(enumerable);
    }
    
    public class Delegates
    {
        public delegate T CreateNodeFromName<out T>(string name);
        public delegate T AddChildrenToNode<T>(T previous, IEnumerable<T> childrenToAdd);
        public delegate IEnumerable<T> GetChildren<T>(T parent);
        public delegate IEnumerable<T> GetDependencyList<T>(T parent);
        public delegate IEnumerable<string> GetDataList<in T>(T parent);
    }
}
