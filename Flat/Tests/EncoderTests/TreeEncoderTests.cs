using System.Collections.Generic;
using System.Linq;
using Flat;
using Flat.Encoders;
using FluentAssertions;
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
            
            res[0].Should().Be("@Client:");
            res[1].Should().Be("@Lib:");
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
            
            res[0].Should().Be("@Client:");
            res[1].Should().Be("@Lib:");
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

            res[0].Should().Be("@Client:");
            res[1].Should().Be("@Client\\Windows:");
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

            res[0].Should().Be("@Client:");
            res[1].Should().Be("@Client\\Windows:");
            res[2].Should().Be("@Client\\Windows\\10:");
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

            res[0].Should().Be("@Client:");
            res[1].Should().Be("@Client\\Linux:");
            res[2].Should().Be("@Client\\Windows:");
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

            res[0].Path.Should().Be("Client");
            res[1].Path.Should().Be("Client\\Linux");
            res[2].Path.Should().Be("Client\\Windows");
            res[3].Path.Should().Be("Lib");
            res[4].Path.Should().Be("Lib\\Common");
            res[5].Path.Should().Be("Lib\\Logic");
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

            res[0].Path.Should().Be("AA");
            res[1].Path.Should().Be("AA\\EE");
            res[2].Path.Should().Be("AA\\FF");
            res.ElementAt(3).Path.Should().Be("BB");
            res.ElementAt(4).Path.Should().Be("BB\\CC");
            res.ElementAt(5).Path.Should().Be("BB\\DD");
        }
    }
}
