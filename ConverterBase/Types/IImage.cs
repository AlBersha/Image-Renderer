namespace ConverterBase
{
    using System.Collections.Generic;

    public interface IImage
    {
        public List<List<Pixel>> Data { get; set; }
    }
}