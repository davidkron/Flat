using System.Collections.Generic;

namespace Flat
{
    internal class FlatEntry
    {
        public string Path;
        public IReadOnlyCollection<string> ChildData;
    }
}