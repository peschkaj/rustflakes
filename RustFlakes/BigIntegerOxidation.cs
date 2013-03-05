using System;
using System.Numerics;

namespace RustFlakes
{
    public class BigIntegerOxidation : Oxidation<BigInteger>
    {
        public const ulong IdentifierMask = 0xFFFFFFFFFFFF;

        private readonly ulong _identifier;

        /// <summary>
        /// Initialize the oxidizer using supplied worker id and the default epoch (1/1/2013)
        /// </summary>
        /// <param name="identifier">The 48-bit worker id</param>
        public BigIntegerOxidation(ulong identifier)
            : this(identifier, DefaultEpoch)
        { }

        /// <summary>
        /// Initialize the oxidizer using a 32-bit worker id
        /// </summary>
        /// <param name="identifier">The 48-bit worker id</param>
        /// <param name="epoch">The epoch used to source the 48-bit time counter</param>
        public BigIntegerOxidation(ulong identifier, DateTime epoch)
            : base(epoch)
        {
            _identifier = identifier & IdentifierMask;
        }

        public BigIntegerOxidation(byte[] identifier)
            : this(identifier, DefaultEpoch)
        { }

        public BigIntegerOxidation(byte[] identifier, DateTime epoch)
            : this(identifier, epoch, false)
		{ }

        public BigIntegerOxidation(byte[] identifier, DateTime epoch, bool littleEndian)
            : base(epoch)
        {
            if (identifier.Length != 6) 
                throw new ArgumentException("Invalid worker identifier length. Should be 6 bytes long", "identifier");

            if (BitConverter.IsLittleEndian != littleEndian)
            {
                identifier = (byte[]) identifier.Clone();
                Array.Reverse(identifier);
            }

            var input = new byte[8];
            Array.Copy(identifier, 0, input, 0, 6);

            _identifier = BitConverter.ToUInt64(input, 0) & IdentifierMask;
        }

        /// <summary>
        /// Generates the next key
        /// </summary>
        /// <returns></returns>
        public override BigInteger Oxidize()
        {
            Update();

            // Initialize a BigInteger with a little-endian byte array
            var result = new byte[16];
            
            // first 2 bytes are the key space counter
            result[0] = (byte)_counter;
            result[1] = (byte)(_counter >> 8);

            // next 6-bytes are the worker id
            for (var i=0; i<6; i++)
                result[i+2] = (byte)(_identifier >> (i*8));

            // next 8 bytes are the time counter
            for (var i=0; i<8; i++)
                result[i+8] = (byte)(_lastOxidizedInMs >> (i*8));

            return new BigInteger(result);
        }
    }
}
