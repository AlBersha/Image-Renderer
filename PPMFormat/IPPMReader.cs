namespace PPMFormat
{
    using System.Collections.Generic;

    public interface IPPMReader
    {
        public List<List<Pixel>> ReadPPM(string path);
    }
}