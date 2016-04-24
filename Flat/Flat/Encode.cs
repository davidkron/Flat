using System;
using System.Collections.Generic;
using System.Linq;
using Flat.Encoders;

namespace Flat
{
    public class Encode
    {
        public static string HierarchicalGraph<T>(IEnumerable<T> nodes, HierarchicalGraphEncoder.GetChildren<T> childAccecor,
            Func<T, string> nameAccessor, HierarchicalGraphEncoder.GetDependencyList<T> dataAccessor)
        {
            return nodes.ToList().EncodeHierarchicalGraph(childAccecor, nameAccessor, dataAccessor);
        }

        public static string Tree<T>(IEnumerable<T> nodes, TreeEncoder.GetChildren<T> childAccecor,
            Func<T, string> nameAccessor, TreeEncoder.GetDataList<T> dataAccessor)
        {
            return nodes.EncodeTree(childAccecor, nameAccessor, dataAccessor);
        }
    }
}
