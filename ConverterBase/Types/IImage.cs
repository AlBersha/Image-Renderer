namespace ConverterBase
{
    using System.Collections.Generic;

    public interface IImage
    {
        public List<List<RGB>> Data { get; set; }
    }
}