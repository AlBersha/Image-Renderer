namespace BMPReader
{
    using System;
    using System.Collections.Generic;
    using ConverterBase;

    public struct BMPHeader
    {
        public byte[] bfType;
        public int bfSize;
        public short[] bfReserved;
        public Int32 bfHeadersize;
        public Int32 biSize;
        public Int32 biWidth;
        public Int32 biHeight;
        public Int16 biPlanes;
        public Int16 biBitCount;
        public Int32 biCompression;
        public Int32 biSizeImage;
        
        public Int32 biXPelsPerMeter; 
        public Int32 biYPelsPerMeter; 
        public Int32 biClrUsed;       
        public Int32 biClrImportant;
    }
    
    public class BMP: IImage
    {
        public BMPHeader BmpHeader;
        public int Width { get; set; }
        public int Height { get; set; }
        public byte BitDepth { get; set; }
        public List<Pixel> Data { get; set; }

        public BMP()
        {
            BmpHeader.bfReserved = new short[2];
            Data = new List<Pixel>();
        }
    }
}