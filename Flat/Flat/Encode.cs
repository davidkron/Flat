using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat.Encoders;

namespace Flat
{
    public class Encode
    {
        public static string Graph<T>(IEnumerable<T> nodes, GraphEncoder.GetChildren<T> childAccecor,
            Func<T, string> nameAccessor, GraphEncoder.GetDependencyList<T> dataAccessor)
        {
            return nodes.EncodeGraph(childAccecor, nameAccessor, dataAccessor);
        }

        public static string Tree<T>(IEnumerable<T> nodes, TreeEncoder.GetChildren<T> childAccecor,
            Func<T, string> nameAccessor, TreeEncoder.GetDataList<T> dataAccessor)
        {
            return nodes.EncodeTree(childAccecor, nameAccessor, dataAccessor);
        }
    }
}
