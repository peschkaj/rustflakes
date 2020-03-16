using System;
using System.Numerics;

namespace RustFlakes
{
    /// <summary>
    /// This utilizations the UInt64Oxidation but moves data from ulong
    /// </summary>
    public class SqlServerBigIntOxidation : UInt64Oxidation
    {
        public SqlServerBigIntOxidation(ushort identifier) : base(identifier)
        {
        }

        public SqlServerBigIntOxidation(ushort identifier, ushort oxidationIntervalInMs) : base(identifier, oxidationIntervalInMs)
        {
        }

        public SqlServerBigIntOxidation(ushort identifier, DateTime epoch) : base(identifier, epoch)
        {
        }

        public SqlServerBigIntOxidation(ushort identifier, DateTime epoch, ushort oxidationIntervalInMs) : base(identifier, epoch, oxidationIntervalInMs)
        {
        }

        public new long Oxidize()
        {
            var o = base.Oxidize();
            return (long)(o - long.MaxValue);
        }
    }
}
