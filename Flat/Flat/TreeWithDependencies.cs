using System.Collections.Generic;
using System.Linq;

namespace Flat
{
    public class TreeWithDependencies<TNode>
    {
        public readonly ISet<TNode> Tree;
        public readonly ILookup<TNode, TNode> Dependencies;

        public TreeWithDependencies(ISet<TNode> tree, ILookup<TNode, TNode> dependencies)
        {
            Tree = tree;
            Dependencies = dependencies;
        }
    }
}