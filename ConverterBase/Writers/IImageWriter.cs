namespace ConverterBase.Writers
{
    public interface IImageWriter
    {
        public string Write(IImage image, string path);
    }
}