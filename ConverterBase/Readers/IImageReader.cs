namespace ConverterBase.Readers
{
    public interface IImageReader
    {
        public IImage ReadPPM(string path);
    }
}