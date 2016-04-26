// This project can output the Class library as a NuGet Package.
// To enable this option, right-click on the project and select the Properties menu item.
//In the Build tab select "Produce outputs on build".

using System;
using System.Collections.Generic;
using System.Linq;

namespace Flat.Encoders
{
    internal static class TreeEncoder
    {   

        public static IEnumerable<FlatEntry> FlattenTree<T>(this IEnumerable<T> nodes, Delegates.GetChildren<T> childAccecor, Func<T, string> nameAccessor, Delegates.GetDataList<T> dataAccessor, string path = "")
        {
            var entries = new List<FlatEntry>();
            foreach (var node in nodes.OrderBy(nameAccessor))
            {
                entries.Add(new FlatEntry
                {
                    Path = path + nameAccessor(node),
                    ChildData = dataAccessor(node).ToList()
                });
                var newPath = path + nameAccessor(node) + "\\";
                entries.AddRange(childAccecor(node).FlattenTree(childAccecor, nameAccessor, dataAccessor, newPath));
            }
            return entries;
        }


        public static string EncodeTree<T>(this IEnumerable<T> nodes, Delegates.GetChildren<T> childAccecor, Func<T, string> nameAccessor, Delegates.GetDataList<T> dataAccessor, string path = "")
        {
            var listToEncode = nodes.FlattenTree(childAccecor, nameAccessor, dataAccessor);
            return FlatListSerializer.EncodeList(listToEncode);
        }
    }
}