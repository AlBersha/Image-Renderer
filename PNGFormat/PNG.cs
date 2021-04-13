namespace PNGFormat
{
    using System;
    using System.Collections.Generic;
    using ConverterBase;

    public class PNG: IImage
    {
        public byte[] Signature { get; set; }
        public List<Chunk> Chunks { get; set; }
        public ImageHeader Header { get; }
        public List<List<Pixel>> Data { get; set; }
        private readonly RawPngData data;

        public PNG()
        {
            Signature = new byte[4];
            Chunks = new List<Chunk>();
            Data = new List<List<Pixel>>();
        }
        
        public PNG(ImageHeader header, RawPngData data)
        {
            Header = header;
            Data = new List<List<Pixel>>();
            this.data = data;
        }
        public Pixel GetPixel(int x, int y) => data.GetPixel(x, y);
    }
}