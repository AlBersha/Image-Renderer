using ConverterBase.Readers;

namespace ConverterBase
{
    using System;
    using Readers;
    using Writers;

    public class Converter : IConverter
    {
        public IImageReader ImageReader { get; set; }
        public IImageWriter ImageWriter { get; set; }
        
        public Converter(IImageReader imageReader, IImageWriter imageWriter)
        {
            ImageReader = imageReader;
            ImageWriter = imageWriter;
        }
        
        public string Convert(string path)
        {
            var imageData = ImageReader.Read(path);
            ImageWriter.Write(imageData,path);

            return path;
            
        }
    }
}
