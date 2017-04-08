using System;
using System.Collections.Generic;
using System.Linq;

namespace Flat.Encoders
{
    internal static class HierarchicalGraphEncoder
    {

        private static Dictionary<T,string> GetFullLengthNodeNames<T>(this IEnumerable<T> nodes, Delegates.GetChildren<T> childAccecor, Func<T, string> nameAccessor, string path = "")
        {
            var nodesDict = new Dictionary<T,string>();
            foreach (var node in nodes.OrderBy(nameAccessor))
            {
                nodesDict[node] = path + nameAccessor(node);
                var newPath = path + nameAccessor(node) + "\\";
                nodesDict =
                    nodesDict.Union(childAccecor(node).GetFullLengthNodeNames(childAccecor, nameAccessor, newPath)).
                        ToDictionary(x => x.Key, x => x.Value);
            }
            return nodesDict;
        }

        public static string EncodeHierarchicalGraph<T>(this IReadOnlyCollection<T> nodes, Delegates.GetChildren<T> childAccecor, Func<T, string> nameAccessor, Delegates.GetDependencyList<T> dataAccessor)
        {
            var listToEncode = FlattenGraph(nodes, childAccecor, nameAccessor, dataAccessor);
            return FlatListSerializer.EncodeList(listToEncode);
        }

        public static IEnumerable<FlatEntry> FlattenGraph<T>(this IReadOnlyCollection<T> nodes, Delegates.GetChildren<T> childAccecor, Func<T, string> nameAccessor, Delegates.GetDependencyList<T> dataAccessor)
        {
            var names = nodes.GetFullLengthNodeNames(childAccecor, nameAccessor);
            var listToEncode = nodes.FlattenGraph(childAccecor, nameAccessor, dataAccessor, names);
            return listToEncode;
        }

        private static IEnumerable<FlatEntry> FlattenGraph<T>(this IEnumerable<T> nodes, Delegates.GetChildren<T> childAccecor, Func<T, string> nameAccessor, Delegates.GetDependencyList<T> dataAccessor, Dictionary<T, string> namesDict)
        {
            var entries = new List<FlatEntry>();
            foreach (var node in nodes.OrderBy(nameAccessor))
            {
                entries.Add(new FlatEntry
                {
                    Path = namesDict[node],
                    ChildData = dataAccessor(node).Select(n => namesDict[n]).ToList()
                });
                entries.AddRange(childAccecor(node).FlattenGraph(childAccecor, nameAccessor, dataAccessor,namesDict));
            }
            return entries;
        }
    }
}