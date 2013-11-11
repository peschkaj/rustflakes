using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using NUnit.Framework;
using RustFlakes;

namespace RustFlakesTests
{
    [TestFixture]
    public class LexicographicExtensionTests
    {
        // 48-bits = 12 hex digits 
        internal ulong WorkerId;

        [SetUp]
        public void Setup()
        {
            WorkerId = 0x12345678abcd;
        }

        [Test]
        public void ShouldMaintain48BitWorkerIdFromULong()
        {
            var oxidation = new BigIntegerOxidation(WorkerId);
            var key = oxidation.Oxidize();
            var strKey = key.ToLexicographicId();

            Assert.AreEqual(strKey.Length, 39);
            Assert.AreEqual(BigInteger.Parse(strKey, NumberStyles.AllowLeadingWhite), key);
        }
    }
}
