using System;
using System.Collections.Generic;
using System.Linq;
using Flat.Encoders;

namespace Flat
{
    public static class Encode
    {
        public static string HierarchicalGraph<T>(IEnumerable<T> nodes, Delegates.GetChildren<T> childAccecor,
            Func<T, string> nameAccessor, Delegates.GetDependencyList<T> dataAccessor)
        => nodes.ToList().EncodeHierarchicalGraph(childAccecor, nameAccessor, dataAccessor);

        public static string Tree<T>(IEnumerable<T> nodes, Delegates.GetChildren<T> childAccecor,
            Func<T, string> nameAccessor, Delegates.GetDataList<T> dataAccessor)
            => nodes.EncodeTree(childAccecor, nameAccessor, dataAccessor);
    }
}
