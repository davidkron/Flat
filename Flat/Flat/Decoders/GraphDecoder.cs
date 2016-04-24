using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat.Encoders;

namespace Flat.Decoders
{
    public class TreeWithDependencies<TNode>
    {
        public readonly ISet<TNode> Nodes;
        public readonly ILookup<TNode, TNode> Edges;

        public TreeWithDependencies(ISet<TNode> nodes, ILookup<TNode, TNode> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }

    public static class GraphDecoder
    {
        public static TreeWithDependencies<TNode> Decode<TNode>(string text,
            CreateNodeFromName<TNode> createNodeWithName,
            AddChildrenToNode<TNode> addChildrenToNode)
        {
            var nodesByPath = new Dictionary<string, TNode>();
            var flats = FlatListSerializer.DecodeFlatlist(text);
            var nodes = GetTreeFromFlatlist(flats, nodesByPath, 0, createNodeWithName, addChildrenToNode).ToList();
            var deps = GetDependenciesFromFlatList(flats, nodesByPath);
            return new TreeWithDependencies<TNode>(nodes.ToSet(), deps);
        }


        private static ILookup<TNode, TNode> GetDependenciesFromFlatList<TNode>(IEnumerable<FlatEntry> flats,
            IReadOnlyDictionary<string, TNode> nodesByPath)
        {
            var edgeList =
                (from flatEntry in flats
                 from child in flatEntry.ChildData
                 select Tuple.Create(nodesByPath[flatEntry.Path], nodesByPath[child])).ToList();
            return edgeList.ToLookup(x => x.Item1, x => x.Item2);
        }

        public delegate T CreateNodeFromName<out T>(string name);
        public delegate T AddChildrenToNode<T>(T previous, IEnumerable<T> childrenToAdd);

        private static IEnumerable<TNode> GetTreeFromFlatlist<TNode>(IReadOnlyCollection<FlatEntry> descendants,
            IDictionary<string, TNode> nodesByPath, int currentDepth,
            CreateNodeFromName<TNode> createNodeWithName,
            AddChildrenToNode<TNode> addChildrenToNode)
        {
            var isDirectChildren = descendants.ToLookup(X => X.Depth() == currentDepth);
            var notDirectChildren = isDirectChildren[false].ToMutableSet();
            var directChildren = isDirectChildren[true];
            foreach (var directChild in directChildren)
            {
                var path = directChild.Path;
                var name = GetNameFromPath(path);
                var topNode = createNodeWithName(name);
                var subDescendants = notDirectChildren.Where(x => x.IsDescendantTo(directChild)).ToList();
                notDirectChildren.ExceptWith(subDescendants);
                var withChildren = addChildrenToNode(topNode,
                    GetTreeFromFlatlist(subDescendants, nodesByPath, currentDepth + 1, createNodeWithName, addChildrenToNode));
                nodesByPath.Add(path, withChildren);
                yield return withChildren;
            }
            Debug.Assert(notDirectChildren.Count == 0);
        }


        private static bool IsDescendantTo(this FlatEntry possibleDescendant, FlatEntry possibleAncestor)
            => possibleDescendant.Path.StartsWith(possibleAncestor.Path + "\\");

        private static int Depth(this FlatEntry x) => x.Path.Count(c => c == '\\');

        private static string GetNameFromPath(string path) => path.Split('\\').Last();
    }
}
