// Copyright (c) 2013 - Jeremiah Peschka
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
using System.Linq;

namespace RustFlakes
{
    public class Oxidation
    {
		private readonly DateTime _epoch;

		private ulong _lastOxidizedInMs;
		private readonly UInt32 _identifier;
		private UInt16 _counter;

		public Oxidation(byte[] identifier)
			: this(identifier, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
        {}

		public Oxidation(byte[] identifier, DateTime epoch)
		{
			_epoch = epoch;

			_lastOxidizedInMs = CurrentTimeCounter();

			if (BitConverter.IsLittleEndian)
				Array.Reverse(identifier);

			// shorten identifier to 6 bytes
			if (identifier.Length > 6)
				identifier = identifier.Take(6).ToArray();

			_identifier = BitConverter.ToUInt32(identifier, 0);
			_counter = 0;
		}


		public decimal Oxidize()
		{
			HandleTime();

			/* Shift _identifier to the left by 48 bits to occupy the appropriate
			 * position in the Decimal.
			 */
			var i = _identifier << 48;

			/* Add UInt64.MaxValue to lastOxidizedInMs to bit shift by 64 bits.
			 * Convert to decimal first to avoid bits wrapping around in the background.
			 * 
			 * Add the shifted identifier
			 * Add the counter
			 */
			return (_lastOxidizedInMs + (decimal) UInt64.MaxValue) + i + _counter;
		}


		private void HandleTime()
		{
			var ct = CurrentTimeCounter();

			if (_lastOxidizedInMs < ct)
			{
				_lastOxidizedInMs = ct;
				_counter = 0;
			}
			else if (_lastOxidizedInMs > ct)
			{
				throw new ApplicationException("Clock is running backwards");
			}
			else
			{
				_counter++;
			}
		}

		private ulong CurrentTimeCounter()
		{
			return (ulong) (DateTime.UtcNow - _epoch).TotalMilliseconds;
		}
	}
}