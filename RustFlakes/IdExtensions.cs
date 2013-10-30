using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RustFlakes
{
    public static class IdExtensions
    {
        // Most NoSQL storage technologies order records by a single Id field
        // that is lexicographically sorted. This will cause flake-ids to be
        // incorrectly sorted. In order to handle this, this extension class
        // provides the ability to front-pad an id with 0s so that it will sort
        // correctly in a lexicographic system.

        //Maximum 128 bit integer:               340282366920938463463374607431768211455
        //                                       0        1         2         3
        //                                       123456789012345678901234567890123456789
        public static readonly string PADDING = "000000000000000000000000000000000000000";
        public static string ToLexicographicId(this BigInteger id)
        {
            var stringId = id.ToString();
            var padding = PADDING.ToCharArray();
            stringId.CopyTo(0, padding, 39 - stringId.Length, stringId.Length);
            return new string(padding);
        }

        public static BigInteger ToBigInteger(this string id)
        {
            return BigInteger.Parse(id, NumberStyles.AllowLeadingWhite);
        }
    }
}
