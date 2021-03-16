namespace BMPReader
{
    using System;
    using System.Collections.Generic;
    using ConverterBase;

    public struct BMPHeader
    {
        public Int32 bfType;
        public int bfSize;
        public int bfReserved;
        public int bfOffBits;
        public int biSize;
        public int biWidth;
        public int biHeight;
        public Int32 biPlanes;
        public Int32 biBitCount;
        public int biCompression;
        public int biSizeImage;
    }
    
    public class BMP: IImage
    {
        public BMPHeader BmpHeader;
        public List<List<RGB>> Data { get; set; }
    }
}