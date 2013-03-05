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

namespace ConsoleFlakes
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			TestOxidations();
		    TestDecimalOxidations();
            TestUInt64Oxidations();

            Console.WriteLine("Press enter");
		    Console.ReadLine();
		}

        internal static void TestOxidations()
        {
            var o = new RustFlakes.BigIntegerOxidation(new byte[] { 0, 1, 2, 3, 4, 5 });

            var start = DateTime.UtcNow;

            for (var i = 0; i < 65536; i++)
            {
                o.Oxidize();
            }

            var finish = DateTime.UtcNow;
            var diff = finish - start;
            Console.WriteLine("BigInteger: Generated 65536 oxidations in {0}ms", diff.TotalMilliseconds);
        }

        internal static void TestDecimalOxidations()
        {
            var o = new RustFlakes.DecimalOxidation(1);

            var start = DateTime.UtcNow;

            for (var i = 0; i < 65536; i++)
            {
                o.Oxidize();
            }

            var finish = DateTime.UtcNow;
            var diff = finish - start;
            Console.WriteLine("Decimal:  Generated 65536 oxidations in {0}ms", diff.TotalMilliseconds);
        }
        internal static void TestUInt64Oxidations()
        {
            var o = new RustFlakes.UInt64Oxidation(1);

            var start = DateTime.UtcNow;

            for (var i = 0; i < 65536; i++)
            {
                o.Oxidize();
            }

            var finish = DateTime.UtcNow;
            var diff = finish - start;
            Console.WriteLine("64-bit:  Generated 65536 oxidations in {0}ms", diff.TotalMilliseconds);
        }
	}
}