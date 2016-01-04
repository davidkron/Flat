// This project can output the Class library as a NuGet Package.
// To enable this option, right-click on the project and select the Properties menu item.
//In the Build tab select "Produce outputs on build".

using System;
using System.Collections.Generic;
using System.Linq;

namespace Flat
{
    public static class Encoder
    {

        public delegate IEnumerable<T> GetChildren<T>(T parent);
        public delegate IEnumerable<string> GetDataList<in T>(T parent);
        

        public static string Flatten<T>(this IEnumerable<T> nodes,
        GetChildren<T> childAccecor,Func<T,string> nameAccessor,GetDataList<T> dataAccessor,string path = "")
        {
            var output = "";
            foreach (var node in nodes.OrderBy(nameAccessor))
            {
                output += EncodeSingleNode(path + nameAccessor(node));
                var childNodes = childAccecor(node);
                var newPath = path;
                if (newPath.Any())
                    newPath += "\\";
                newPath += nameAccessor(node) + "\\";
                output += childNodes.Flatten(childAccecor, nameAccessor, dataAccessor, newPath);
            }
            return output;
        }

        public static string EncodeSingleNode(string name)
        {
            return "[" + name + "]" + "\n";
        }

        public static string FlattenNodeName(string name, string path)
        {
            return path + "\\" + name;
        }
    }
}