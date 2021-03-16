namespace PPMFormat
{
    using System.Collections.Generic;
    using ConverterBase;

    public class PPM: IImage
    {
        public string Type;
        public string Comments;
        public int Width;
        public int Height;
        public int ColorRange;
        public List<List<RGB>> Data { get; set; }

        public PPM()
        {
            Type = "";
            Comments = "";
            Width = 0;
            Height = 0;
            ColorRange = 0;
            Data = new List<List<RGB>>();
        }

    }
}