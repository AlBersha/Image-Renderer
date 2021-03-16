namespace PPMFormat
{
    using System.Collections.Generic;
    using System.Net.Mime;
    using ConverterBase;
    using ConverterBase.Writers;

    public interface IPPMWriter: IImageWriter
    {
        public bool WriteImage(IImage imageData, string outputPath);
    }
}