using System.Collections.Generic;
using System.Linq;
using Flat.Encoders;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EncoderTests
{
    [TestClass]
    public class GraphEncoderTests
    {
        [TestMethod]
        public void TestDependenciesAreResolvedToFullName()
        {
            var logic = new TestGraphNode("Logic");

            var root = new List<TestGraphNode>
            {
                new TestGraphNode
                {
                    Name = "Client",
                    Dependencies = { logic }
                },
                new TestGraphNode
                {
                    Name = "Lib",
                    Children =
                    {
                        logic
                    }
                }
            };

            var res = root.FlattenGraph(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Dependencies).ToArray();

            res[0].Path.Should().Be("Client");
            res[0].ChildData.First().Should().Be("Lib\\Logic");
        }

        [TestMethod]
        public void GraphFlattener()
        {
            var root = new List<TestGraphNode>
            {
                new TestGraphNode
                {
                    Name = "AA",
                    Children =
                    {
                        new TestGraphNode("EE"),
                        new TestGraphNode("FF")
                    }
                },
                new TestGraphNode
                {
                    Name = "BB",
                    Children =
                    {
                        new TestGraphNode("DD"),
                        new TestGraphNode("CC")
                    }
                }
            };
            var res = root.FlattenGraph(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Dependencies).ToArray();

            // Assert
            res[0].Path.Should().Be("AA");
            res[1].Path.Should().Be("AA\\EE");
            res[2].Path.Should().Be("AA\\FF");
            res[3].Path.Should().Be("BB");
            res[4].Path.Should().Be("BB\\CC");
            res[5].Path.Should().Be("BB\\DD");
        }
    }
}
