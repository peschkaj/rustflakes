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
using System.IO;
using NUnit.Framework;
using RustFlakes;

namespace RustFlakesTests
{
    [TestFixture]
    public class DecimalOxidationTests
    {
        internal uint WorkerId;

        [SetUp]
        public void Setup()
        {
            WorkerId = 0xfedcba98;
        }

        [Test]
        public void ShouldMaintain32BitWorkerId()
        {
            var oxidation = new DecimalOxidation(WorkerId);
            var key = oxidation.Oxidize();

            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream))
                writer.Write(key);

            var result = stream.ToArray();
            var id = BitConverter.ToUInt32(result, 2);

            Assert.AreEqual(WorkerId, id);
        }

        [Test]
        public void SequentialKeysAreSequential()
        {
            var oxidation = new DecimalOxidation(WorkerId);
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