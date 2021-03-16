namespace ConverterBase.Writers
{
    using System.Collections.Generic;

    public interface IImageWriter
    {
        public bool WriteImage(IImage image, string outputPath);
    }
}