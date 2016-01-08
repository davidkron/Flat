using System;
using System.Collections.Generic;
using System.Linq;
using Flat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EncoderTests
{
    [TestClass]
    public class GraphEncoderTests
    {

        class TestNode
        {
            public string Name { get; set; }
            public TestNode(string name)
            {
                Name = name;
            }

            public TestNode()
            {
            }

            public List<TestNode> Children { get; } = new List<TestNode>();
            public List<TestNode> Dependencies { get;} = new List<TestNode>();
        }

        [TestMethod]
        public void TestDependenciesAreResolvedToFullName()
        {
            var logic = new TestNode("Logic");

            var root = new List<TestNode>
            {
                new TestNode
                {
                    Name = "Client",
                    Dependencies = { logic }
                },
                new TestNode
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

            Assert.AreEqual("Client", res[0].name);
            Assert.AreEqual("Lib\\Logic", res[0].childData.First());
        }

        [TestMethod]
        public void GraphFlattener()
        {
            var root = new List<TestNode>
            {
                new TestNode
                {
                    Name = "AA",
                    Children =
                    {
                        new TestNode("EE"),
                        new TestNode("FF")
                    }
                },
                new TestNode
                {
                    Name = "BB",
                    Children =
                    {
                        new TestNode("DD"),
                        new TestNode("CC")
                    }
                }
            };
            var res = root.FlattenGraph(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Dependencies).ToArray();

            Assert.AreEqual("AA", res[0].name);
            Assert.AreEqual("AA\\EE", res[1].name);
            Assert.AreEqual("AA\\FF", res[2].name);
            Assert.AreEqual("BB", res.ElementAt(3).name);
            Assert.AreEqual("BB\\CC", res.ElementAt(4).name);
            Assert.AreEqual("BB\\DD", res.ElementAt(5).name);
        }
    }
}
