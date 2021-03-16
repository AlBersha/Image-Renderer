using System;

namespace Renderer
{
    using BMPReader;
    using ConverterBase;

    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\Alexandra\\Desktop\\do_it.bmp";
            
            // PPMReader ppmreader = new PPMReader();
            // IImage image = ppmreader.ReadImage(path);
            
            // string outputPath = "D:\\KPI\\#graphics\\result.ppm";
            // PPMWriter ppmWriter = new PPMWriter();
            // ppmWriter.WriteImage(image, outputPath);
            
            BMPReader bmpReader = new BMPReader();
            IImage image = bmpReader.ReadImage(path);
        }
    }
}