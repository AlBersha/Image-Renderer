namespace ConverterBase.Readers
{
    public interface IImageReader
    {
        public IImage Read(string path);
    }
}