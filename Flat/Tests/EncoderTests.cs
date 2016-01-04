using System;
using System.Collections.Generic;
using System.Linq;
using Flat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class EncoderTests
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
            public List<string> Data { get; set; }
        }

        [TestMethod]
        public void EncodeTwoRootEntrys()
        {
            var root = new TestNode();
            root.Children.Add(new TestNode("Lib"));
            root.Children.Add(new TestNode("Client" ));
            var res = root.Children.Flatten(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');
            
            Assert.AreEqual("[Client]", res[0]);
            Assert.AreEqual("[Lib]", res[1]);
        }

        [TestMethod]
        public void EncodeNestedEntry()
        {
            var root = new TestNode();
            root.Children.Add(new TestNode
            {
                Name = "Client" , Children = { new TestNode("Windows") }
            });
            var res = root.Children.Flatten(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("[Client]", res[0]);
            Assert.AreEqual("[Client\\Windows]", res[1]);
        }
        [TestMethod]
        public void EncodeTwoNestedEntries()
        {
            var root = new TestNode();
            root.Children.Add(new TestNode
            {
                Name = "Client",
                Children =
                {
                    new TestNode("Windows"), 
                    new TestNode("Linux")
                }
            });
            var res = root.Children.Flatten(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("[Client]", res[0]);
            Assert.AreEqual("[Client\\Linux]", res[1]);
            Assert.AreEqual("[Client\\Windows]", res[2]);
        }

        [TestMethod]
        public void EncodeFourNestedEntrys()
        {
            var root = new TestNode();
            root.Children.Add(new TestNode
            {
                Name = "Client",
                Children =
                {
                    new TestNode("Windows"),
                    new TestNode("Linux")
                }
            });
            root.Children.Add(new TestNode
            {
                Name = "Lib",
                Children =
                {
                    new TestNode("Logic"),
                    new TestNode("Common")
                }
            });
            var res = root.Children.Flatten(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("[Client]", res[0]);
            Assert.AreEqual("[Client\\Linux]", res[1]);
            Assert.AreEqual("[Client\\Windows]", res[2]);
            Assert.AreEqual("[Lib]", res[3]);
            Assert.AreEqual("[Lib\\Common]", res[4]);
            Assert.AreEqual("[Lib\\Logic]", res[5]);
        }

        [TestMethod]
        public void AlphabeticSort()
        {
            var root = new TestNode();
            root.Children.Add(new TestNode
            {
                Name = "AA",
                Children =
                {
                    new TestNode("EE"),
                    new TestNode("FF")
                }
            });
            root.Children.Add(new TestNode
            {
                Name = "BB",
                Children =
                {
                    new TestNode("DD"),
                    new TestNode("CC")
                }
            });
            var res = root.Children.Flatten(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("[AA]", res[0]);
            Assert.AreEqual("[AA\\EE]", res[1]);
            Assert.AreEqual("[AA\\FF]", res[2]);
            Assert.AreEqual("[BB]", res[3]);
            Assert.AreEqual("[BB\\CC]", res[4]);
            Assert.AreEqual("[BB\\DD]", res[5]);
        }
    }
}
