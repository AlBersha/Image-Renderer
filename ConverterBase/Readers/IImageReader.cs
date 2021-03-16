namespace ConverterBase.Readers
{
    public interface IImageReader
    {
        public IImage ReadImage(string path);
    }
}