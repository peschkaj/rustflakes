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

namespace RustFlakes
{
    /// <summary>
    /// RustFlake implementation backed by a 96-bit decimal. Has a 48-bit time counter, a 32-bit worker id, and a 16-bit key space counter
    /// </summary>
    public class DecimalOxidation : Oxidation<decimal>
    {
        private readonly uint _identifier;

        /// <summary>
        /// Initialize the oxidizer using supplied worker id and the default epoch (1/1/2013)
        /// </summary>
        /// <param name="epoch">The epoch used to source the 48-bit time counter</param>
        public DecimalOxidation(uint identifier)
            : this(identifier, DefaultEpoch)
        { }

        /// <summary>
        /// Initialize the oxidizer using a 32-bit worker id
        /// </summary>
        /// <param name="identifier">The 32-bit worker id</param>
        /// <param name="epoch">The epoch used to source the 48-bit time counter</param>
        public DecimalOxidation(uint identifier, DateTime epoch)
            : base(epoch)
        {
            _identifier = identifier;
        }

        /// <summary>
        /// Generates the next key
        /// </summary>
        /// <returns></returns>
        public override decimal Oxidize()
        {
            Update();

            return new decimal(
                (int)((_identifier << 16) + _counter),
                (int)((_lastOxidizedInMs << 16) + (_identifier >> 16)),
                (int)((_lastOxidizedInMs >> 16) & 0xFFFFFFFF),
                false,
                0);
        }
    }
}
