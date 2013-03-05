// Copyright (c) 2013 - Jeremiah Peschka
// Copyright (c) 2013 - Mike Haboustak
//
// This file is provided to you under the Apache License,
// Version 2.0 (the "License"); you may not use this file
// except in compliance with the License.  You may obtain
// a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System;
using NUnit.Framework;
using RustFlakes;

namespace RustFlakesTests
{
    [TestFixture]
    public class BigIntegerOxidationTests
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
            var result = key.ToByteArray();

            var id = BitConverter.ToUInt64(result, 2) & BigIntegerOxidation.IdentifierMask;

            Assert.AreEqual(WorkerId, id);
        }

        [Test]
        public void ShouldMaintain48BitWorkerIdFromLittleEndian()
        {
            // 48-bits = 6 bytes
            var workerId = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06};

            var oxidation = new BigIntegerOxidation(workerId, BigIntegerOxidation.DefaultEpoch, true);
            var key = oxidation.Oxidize();

            var result = key.ToByteArray();
            
            for (var i = 0; i < 6; i++)
                Assert.AreEqual(workerId[i], result[i + 2]);
        }

        [Test]
        public void ShouldMaintain48BitWorkerIdFromBigEndian()
        {
            // 48-bits = 6 bytes
            var workerId = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06};

            var oxidation = new BigIntegerOxidation(workerId);
            var key = oxidation.Oxidize();
            var result = key.ToByteArray();

            for (var i = 0; i < 6; i++)
                Assert.AreEqual(workerId[5 - i], result[i + 2]);
        }

        [Test]
        public void SequentialKeysAreSequential()
        {
            // 32-bits = 8 hex digits 
            var oxidation = new BigIntegerOxidation(0x12345678abcd);
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