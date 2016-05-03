using System.Collections.Generic;
using System.Linq;
using Flat.Decoders;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EncoderTests
{
    class TestGraphNode
    {
        public string Name { get; set; }
        public TestGraphNode(string name)
        {
            Name = name;
        }

        public TestGraphNode()
        {
        }

        public TestGraphNode WithChildren(IEnumerable<TestGraphNode> childs)
            => new TestGraphNode(Name) { Children = childs.ToList() };

        public List<TestGraphNode> Children { get; private set; } = new List<TestGraphNode>();
        public List<TestGraphNode> Dependencies { get; } = new List<TestGraphNode>();
    }

    [TestClass]
    public class GraphDecoderTests
    {

        [TestMethod]
        public void TestDependencyPathsAreResolvedToNames()
        {
            var decodedGraph = HierarchicalGraphDecoder.Decode(
                @"
                @Client:
                Lib\Logic
                @Lib:
                @Lib\Logic:", (name,path) => new TestGraphNode(name)
                , (previous, add) => previous.WithChildren(add));
            var nodes = decodedGraph.Tree;
            var edges = decodedGraph.Dependencies;
            // Assert
            nodes.Should().ContainSingle(x => x.Name == "Client");
            edges[nodes.First(x => x.Name == "Client")].Should().Contain(x => x.Name == "Logic");
        }

        [TestMethod]
        public void UnFlattens()
        {
            var nodes = HierarchicalGraphDecoder.Decode(
                @"
                @AA:
                @AA\EE:
                @AA\FF:
                @BB:
                @BB\CC:
                @BB\DD:", (name, path) => new TestGraphNode(name), (previous, add) => previous.WithChildren(add)).Tree;

            // Assert

            nodes.Should()
                .Contain(x => x.Name == "AA").Which.Children.Should()
                    .Contain(x => x.Name == "EE").And
                    .Contain(x => x.Name == "FF");

            nodes.Should()
                .Contain(x => x.Name == "BB").Which.Children.Should()
                    .Contain(x => x.Name == "DD").And
                    .Contain(x => x.Name == "CC");
        }
    }
}
