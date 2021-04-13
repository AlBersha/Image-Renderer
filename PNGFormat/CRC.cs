namespace PNGFormat
{
    using System.Collections.Generic;

    public class CRC
    {
        private const uint Polynomial = 0xEDB88320;

        private static readonly uint[] Lookup;

        static CRC()
        {
            Lookup = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                var value = i;
                for (var j = 0; j < 8; ++j)
                {
                    if ((value & 1) != 0)
                    {
                        value = (value >> 1) ^ Polynomial;
                    }
                    else
                    {
                        value >>= 1;
                    }
                }

                Lookup[i] = value;
            }
        }
        
        public static uint Calculate(byte[] data)
        {
            var crc32 = uint.MaxValue;
            for (var i = 0; i < data.Length; i++)
            {
                var index = (crc32 ^ data[i]) & 0xFF;
                crc32 = (crc32 >> 8) ^ Lookup[index];
            }

            return crc32 ^ uint.MaxValue;
        }

        public static uint Calculate(List<byte> data)
        {
            var crc32 = uint.MaxValue;
            for (var i = 0; i < data.Count; i++)
            {
                var index = (crc32 ^ data[i]) & 0xFF;
                crc32 = (crc32 >> 8) ^ Lookup[index];
            }

            return crc32 ^ uint.MaxValue;
        }
        
        public static uint Calculate(byte[] data, byte[] data2)
        {
            var crc32 = uint.MaxValue;
            for (var i = 0; i < data.Length; i++)
            {
                var index = (crc32 ^ data[i]) & 0xFF;
                crc32 = (crc32 >> 8) ^ Lookup[index];
            }

            for (var i = 0; i < data2.Length; i++)
            {
                var index = (crc32 ^ data2[i]) & 0xFF;
                crc32 = (crc32 >> 8) ^ Lookup[index];
            }

            return crc32 ^ uint.MaxValue;
        }

    }
}