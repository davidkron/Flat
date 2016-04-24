using System.Collections.Generic;

namespace Flat
{
    public class FlatEntry
    {
        public string Path;
        public IReadOnlyCollection<string> ChildData;
    }
}