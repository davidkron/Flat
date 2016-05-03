using System.Collections.Generic;
using System.Collections.Immutable;

namespace Flat
{

    internal static class Extensions
    {
        internal static ISet<T> ToSet<T>(this IEnumerable<T> enumerable) =>
            enumerable.ToImmutableHashSet();
        internal static HashSet<T> ToMutableSet<T>(this IEnumerable<T> enumerable) =>
            new HashSet<T>(enumerable);
    }
    
    public class Delegates
    {
        public delegate T CreateNodeFromNameAndPath<out T>(string name,string path);
        public delegate T AddChildrenToNode<T>(T previous, IEnumerable<T> childrenToAdd);
        public delegate IEnumerable<T> GetChildren<T>(T parent);
        public delegate IEnumerable<T> GetDependencyList<T>(T parent);
        public delegate IEnumerable<string> GetDataList<in T>(T parent);
    }
}
