namespace PNGFormat
{
    using System.Collections.Generic;
    using ConverterBase;

    public class PNG : IImage
    {
        public List<List<RGB>> Data { get; set; }
    }
}