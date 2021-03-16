namespace BMPReader
{
    using ConverterBase;
    using ConverterBase.Readers;

    public interface IBMPRReader: IImageReader
    {
        public IImage ReadImage(string path);
    }
}