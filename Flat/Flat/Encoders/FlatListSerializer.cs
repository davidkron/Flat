using System.Collections.Generic;
using System.Linq;

namespace Flat.Encoders
{
    public static class FlatListSerializer
    {
        public static string EncodeList(IEnumerable<FlatEntry> entries)
        {
            return string.Join("", entries.Select(EncodeSingleEntry));
        }

        private static string EncodeSingleEntry(FlatEntry entry)
        {
            return EncodeNodeName(entry.name) + EncodeChildren(entry.childData);
        }

        private static string EncodeChildren(IEnumerable<string> childData)
        {
            return string.Join("\n", childData.OrderBy(x => x));
        }

        private static string EncodeNodeName(string name)
        {
            return "@" + name + ":\n";
        }
    }
}