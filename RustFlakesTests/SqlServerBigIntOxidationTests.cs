using NUnit.Framework;
using RustFlakes;

namespace RustFlakesTests
{
    [TestFixture]
    public class SqlServerBigIntOxidationTests
    {
        // 16-bits = 4 hex digits
        internal ushort WorkerId;

        [SetUp]
        public void Setup()
        {
            WorkerId = 0xabcd;
        }

        [Test]
        public void ShouldMaintain16BitWorkerId()
        {
            var oxidation = new SqlServerBigIntOxidation(WorkerId);
            var key = oxidation.Oxidize();
            var id = (ushort)(key >> 16 & 0xffff);

            Assert.AreEqual(WorkerId, id);
        }

        [Test]
        public void SequentialKeysAreSequential()
        {
            var oxidation = new SqlServerBigIntOxidation(WorkerId);

            var key = oxidation.Oxidize();
            var key2 = oxidation.Oxidize();
            var key3 = oxidation.Oxidize();

            System.Threading.Thread.Sleep(10);
            var key4 = oxidation.Oxidize();
            var key5 = oxidation.Oxidize();

            Assert.IsTrue(key5 > key4 && key4 > key3 && key3 > key2 && key2 > key);
        }
    }
}
