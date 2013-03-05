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
    public abstract class Oxidation<T>
    {
        public static readonly DateTime DefaultEpoch = new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        protected ushort Counter;
        protected readonly DateTime Epoch;
        protected ulong LastOxidizedInMs;

        protected Oxidation(DateTime epoch)
        {
            Counter = 0;
            Epoch = epoch;
            LastOxidizedInMs = CurrentTime();
        }

        public abstract T Oxidize();

        protected void Update()
        {
            var timeInMs = CurrentTime();

            if (LastOxidizedInMs > timeInMs)
                throw new ApplicationException("Clock is running backwards");

            Counter = (ushort) ((LastOxidizedInMs < timeInMs) ? 0 : Counter + 1);
            LastOxidizedInMs = timeInMs;
        }

        private ulong CurrentTime()
        {
            return (ulong) (DateTime.UtcNow - Epoch).TotalMilliseconds;
        }
    }
}