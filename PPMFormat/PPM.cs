namespace PPMFormat
{
    using System.Collections.Generic;
    using ConverterBase;

    public class PPM: IImage
    {
        public string Type;
        public string Comments;
        public int Width { get; set; }
        public int Height { get; set; }
        public byte BitDepth { get; set; }
        public List<List<Pixel>> Data { get; set; }

        public PPM()
        {
            Type = "";
            Comments = "";
            Width = 0;
            Height = 0;
            BitDepth = 0;
            Data = new List<List<Pixel>>();
        }

    }
}