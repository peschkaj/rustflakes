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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RustFlakes
{
    [TestClass]
    public class DecimalOxidationTests
    {
        [TestMethod]
        public void Should_maintain_32bit_worker_id()
        {
            // 32-bits = 8 hex digits 
            uint worker_id = 0x12345678;

            var oxidation = new DecimalOxidation(worker_id);
            decimal key = oxidation.Oxidize();
            var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream))
                writer.Write(key);
            var result = stream.ToArray();
            uint id = BitConverter.ToUInt32(result, 2);
            Assert.AreEqual(worker_id, id);
        }

        [TestMethod]
        public void Sequential_keys_are_sequential()
        {
            // 32-bits = 8 hex digits 
            var oxidation = new DecimalOxidation(0xfedcba98);
            decimal key = oxidation.Oxidize();
            decimal key2 = oxidation.Oxidize();
            decimal key3 = oxidation.Oxidize();

            System.Threading.Thread.Sleep(10);
            decimal key4 = oxidation.Oxidize();
            decimal key5 = oxidation.Oxidize();

            Assert.IsTrue(key5 > key4 && key4 > key3 && key3 > key2 && key2 > key);
        }
    }
}