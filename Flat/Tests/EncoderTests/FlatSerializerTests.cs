using System;
using System.Collections.Generic;
using Flat;
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
                    name = "Client",
                    childData = new List<string> {"Lib\\Logic"}
                }
            };
            var res =  Flat.Encoders.FlatListSerializer.EncodeList(flatlist).Split('\n');
            Assert.AreEqual("@Client:",res[0]);
            Assert.AreEqual("Lib\\Logic", res[1]);
        }
    }
}
