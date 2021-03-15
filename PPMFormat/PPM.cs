namespace PPMFormat
{
    using System.Collections.Generic;

    public class PPM
    {
        public string Type;
        public string Comments;
        public int Width;
        public int Height;
        public int ColorRange;
        public List<List<Pixel>> Data;

        public PPM()
        {
            Type = "";
            Comments = "";
            Width = 0;
            Height = 0;
            ColorRange = 0;
            Data = new List<List<Pixel>>();
        }
    }
}