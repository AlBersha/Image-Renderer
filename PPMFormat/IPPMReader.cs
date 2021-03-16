namespace PPMFormat
{
    using System.Collections.Generic;
    using ConverterBase;
    using ConverterBase.Readers;

    public interface IPPMReader: IImageReader
    {
        public IImage ReadImage(string path);
    }
}