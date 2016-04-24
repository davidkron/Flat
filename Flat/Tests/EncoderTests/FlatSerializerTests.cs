using System;
using System.Collections.Generic;
using Flat;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.EncoderTests
{
    [TestClass]
    public class FlatSerializerTests
    {
        [TestMethod]
        public void TestFollowsCorrectSyntax()
        {
            var flatlist = new List<FlatEntry>
            {
                new FlatEntry
                {
                    Path = "Client",
                    ChildData = new List<string> {"Lib\\Logic"}
                }
            };
            var res =  Flat.Encoders.FlatListSerializer.EncodeList(flatlist).Split('\n');
            res[0].Should().Be("@Client:");
            res[1].Should().Be("Lib\\Logic");
        }
    }
}
