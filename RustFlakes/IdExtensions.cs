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

        public static string ToLexicographicId(this BigInteger id)
        {
            return id.ToString().PadLeft (39, '0');
        }

        public static BigInteger ToBigInteger(this string id)
        {
            return BigInteger.Parse(id, NumberStyles.AllowLeadingWhite);
        }
    }
}
