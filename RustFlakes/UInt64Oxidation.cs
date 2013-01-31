﻿// Copyright (c) 2013 - Jeremiah Peschka
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
    public class UInt64Oxidation
    {
        private readonly DateTime _epoch;

        private uint _lastOxidizedInMs;
        private readonly ushort _identifier;
        private ushort _counter;

        // 2/1/0001 12:00:00 AM
        public UInt64Oxidation(ushort identifier)
            : this(identifier, new DateTime(26784000000000, DateTimeKind.Utc))
        { }

        public UInt64Oxidation(ushort identifier, DateTime customEpoch)
        {
            _epoch = customEpoch;

            _lastOxidizedInMs = CurrentTimeCounter();

            _identifier = identifier;

            _counter = 0;
        }

        public UInt64 Oxidize()
        {
            HandleTime();

            return ((ulong) _lastOxidizedInMs << 32) + ((ulong) _identifier << 16) + _counter;
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

        private uint CurrentTimeCounter()
        {
            return (uint) Math.Floor((DateTime.UtcNow - _epoch).TotalMilliseconds);
        }
    }
}
