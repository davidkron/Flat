using System.Collections.Generic;
using System.Linq;
using Flat;
using Flat.Encoders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EncoderTests
{
    [TestClass]
    public class TreeEncoderTests
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
            public List<string> Data { get; set; } = new List<string>();
        }

        [TestMethod]
        public void EncodeTwoRootEntrys()
        {
            var root = new List<TestNode> {new TestNode("Lib"), new TestNode("Client")};
            var res = root.EncodeTree(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');
            
            Assert.AreEqual("@Client:", res[0]);
            Assert.AreEqual("@Lib:", res[1]);
        }

        [TestMethod]
        public void RasterizesDataUnderneath()
        {
            var root = new List<TestNode> {new TestNode("Lib")
            {
                Data = new List<string>()
            }, new TestNode("Client")};
            var res = root.EncodeTree(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("@Client:", res[0]);
            Assert.AreEqual("@Lib:", res[1]);
        }

        [TestMethod]
        public void EncodeNestedEntry()
        {
            var root = new List<TestNode>
            {
                new TestNode
                {
                    Name = "Client",
                    Children = {new TestNode("Windows")}
                }
            };
            var res = root.EncodeTree(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("@Client:", res[0]);
            Assert.AreEqual("@Client\\Windows:", res[1]);
        }

        [TestMethod]
        public void EncodeEntryNestedTwoLevels()
        {
            var root = new List<TestNode>
            {
                new TestNode
                {
                    Name = "Client",
                    Children =
                    {
                        new TestNode("Windows")
                        {
                            Children = { new TestNode("10")}
                        }
                    }
                }
            };
            var res = root.EncodeTree(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("@Client:", res[0]);
            Assert.AreEqual("@Client\\Windows:", res[1]);
            Assert.AreEqual("@Client\\Windows\\10:", res[2]);
        }

        [TestMethod]
        public void EncodeTwoNestedEntries()
        {
            var root = new List<TestNode>
            {
                new TestNode
                {
                    Name = "Client",
                    Children =
                    {
                        new TestNode("Windows"),
                        new TestNode("Linux")
                    }
                }
            };
            var res = root.EncodeTree(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).Split('\n');

            Assert.AreEqual("@Client:", res[0]);
            Assert.AreEqual("@Client\\Linux:", res[1]);
            Assert.AreEqual("@Client\\Windows:", res[2]);
        }

        [TestMethod]
        public void EncodeFourNestedEntrys()
        {
            var root = new List<TestNode>
            {
                new TestNode
                {
                    Name = "Client",
                    Children =
                    {
                        new TestNode("Windows"),
                        new TestNode("Linux")
                    }
                },
                new TestNode
                {
                    Name = "Lib",
                    Children =
                    {
                        new TestNode("Logic"),
                        new TestNode("Common")
                    }
                }
            };
            var res = root.FlattenTree(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).ToArray();

            Assert.AreEqual("Client", res[0].name);
            Assert.AreEqual("Client\\Linux", res[1].name);
            Assert.AreEqual("Client\\Windows", res[2].name);
            Assert.AreEqual("Lib", res[3].name);
            Assert.AreEqual("Lib\\Common", res[4].name);
            Assert.AreEqual("Lib\\Logic", res[5].name);
        }

        [TestMethod]
        public void AlphabeticSort()
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
            var res = root.FlattenTree(
                parent => parent.Children,
                parent => parent.Name,
                parent => parent.Data).ToArray();

            Assert.AreEqual("AA", res[0].name);
            Assert.AreEqual("AA\\EE", res[1].name);
            Assert.AreEqual("AA\\FF", res[2].name);
            Assert.AreEqual("BB", res.ElementAt(3).name);
            Assert.AreEqual("BB\\CC", res.ElementAt(4).name);
            Assert.AreEqual("BB\\DD", res.ElementAt(5).name);
        }
    }
}
