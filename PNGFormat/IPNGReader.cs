namespace PNGFormat
{
    using ConverterBase;
    using ConverterBase.Readers;

    public interface IPNGReader: IImageReader
    {
        public IImage ReadImage(string path);
        
    }
}