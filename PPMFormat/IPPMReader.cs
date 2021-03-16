namespace PPMFormat
{
    using System.Collections.Generic;
    using ConverterBase;

    public interface IPPMReader
    {
        public IImage ReadPPM(string path);
    }
}