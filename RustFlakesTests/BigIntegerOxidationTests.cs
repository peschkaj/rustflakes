using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace RustFlakes
{
    [TestClass]
    public class BigIntegerOxidationTests
    {
        [TestMethod]
        public void Should_maintain_48bit_worker_id_from_ulong()
        {
            // 48-bits = 12 hex digits 
            ulong worker_id = 0x12345678abcd;

            var oxidation = new BigIntegerOxidation(worker_id);
            BigInteger key = oxidation.Oxidize();
            var result = key.ToByteArray();
            ulong id = BitConverter.ToUInt64(result, 2) & BigIntegerOxidation.IdentifierMask;
            Assert.AreEqual(worker_id, id);
        }

        [TestMethod]
        public void Should_maintain_48bit_worker_id_from_littleendian()
        {
            // 48-bits = 6 bytes
            var worker_id = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };

            var oxidation = new BigIntegerOxidation(worker_id, BigIntegerOxidation.DefaultEpoch, true);
            BigInteger key = oxidation.Oxidize();
            var result = key.ToByteArray();
            for (var i = 0; i < 6; i++)
                Assert.AreEqual(worker_id[i], result[i + 2]);
        }

        [TestMethod]
        public void Should_maintain_48bit_worker_id_from_bigendian()
        {
            // 48-bits = 6 bytes
            var worker_id = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };

            var oxidation = new BigIntegerOxidation(worker_id);
            BigInteger key = oxidation.Oxidize();
            var result = key.ToByteArray();
            for (var i=0; i<6; i++)
                Assert.AreEqual(worker_id[5-i], result[i+2]);
        }

        [TestMethod]
        public void Sequential_keys_are_sequential()
        {
            // 32-bits = 8 hex digits 
            var oxidation = new BigIntegerOxidation(0x12345678abcd);
            BigInteger key = oxidation.Oxidize();
            BigInteger key2 = oxidation.Oxidize();
            BigInteger key3 = oxidation.Oxidize();

            System.Threading.Thread.Sleep(10);
            BigInteger key4 = oxidation.Oxidize();
            BigInteger key5 = oxidation.Oxidize();

            Assert.IsTrue(key5 > key4 && key4 > key3 && key3 > key2 && key2 > key);
        }
    }
}
