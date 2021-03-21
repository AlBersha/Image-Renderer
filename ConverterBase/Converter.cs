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
        
        public bool Convert(string sourcePath, string outputPath)
        {
            var imageData = ImageReader.ReadImage(sourcePath);
            ImageWriter.WriteImage(imageData,outputPath);

            return true;
            
        }
    }
}
