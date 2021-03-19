namespace ConverterBase
{
    using System.Collections.Generic;

    public interface IImage
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public byte BitDepth { get; set; }
        public List<List<RGB>> Data { get; set; }
    }
}