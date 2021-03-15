namespace ConverterBase
{
    using Readers;
    using Writers;

    public interface IConverter
    {
        public IImageReader ImageReader { get; set; }
        public IImageWriter ImageWriter { get; set; }
        
        public string Convert();
    }
}