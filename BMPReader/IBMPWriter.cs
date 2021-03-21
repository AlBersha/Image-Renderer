namespace BMPReader
{
    using ConverterBase;
    using ConverterBase.Writers;

    public interface IBMPWriter: IImageWriter
    {
        public bool WriteImage(IImage image, string outputPath);
    }
}