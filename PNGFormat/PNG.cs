namespace PNGFormat
{
    using System.Collections.Generic;
    using ConverterBase;

    public class PNG: IImage
    {
        public byte[] Signature { get; set; }
        public List<Chunk> Chunks { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte BitDepth { get; set; }
        public byte Interlace { get; set; }
        public int Gama { get; set; }
        public List<Pixel> Data { get; set; }

        public PNG()
        {
            Signature = new byte[4];
            Chunks = new List<Chunk>();
            Data = new List<Pixel>();
        }
    }
}