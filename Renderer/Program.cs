using System;

namespace Renderer
{
    using System.Collections;
    using System.Collections.Generic;
    using ConverterBase;
    using ConverterBase.Readers;
    using ConverterBase.Writers;
    using GifFormat;
    using PPMFormat;

    class Program
    {
        static void Main(string[] args)
        {
            // var sourceFile = "";
            // var goalFormat = "";
            // var outputFile = "";
            //
            // foreach (var arg in args)
            // {
            //     if (arg.StartsWith("--source="))
            //     {
            //         sourceFile = arg.Substring(arg.IndexOf('=') + 1);
            //     };
            //     
            //     if (arg.StartsWith("--goal-format"))
            //     {
            //         goalFormat = arg.Substring(arg.IndexOf('=') + 1);
            //     }
            //
            //     if (arg.StartsWith("--output"))
            //     {
            //         outputFile = arg.Substring(arg.IndexOf('=') + 1);
            //     }
            // }
            //
            // if (sourceFile == "")
            // {
            //     Console.WriteLine("Argument --source is either entered incorrectly or is missing");
            // }
            //
            // if (goalFormat == "")
            // {
            //     Console.WriteLine("Argument --source is either entered incorrectly or is missing");
            // }
            //
            // if (outputFile == "")
            // {
            //     outputFile = sourceFile;
            // }

            IImageReader imageReader = new GifReader();
            IImageWriter imageWriter = new PPMWriter();
            //
            // switch (sourceFile)
            // {
            //     case "gif":
            //     {
            //         imageReader = new GifReader();
            //         break;
            //     }
            // }
            //
            // switch (goalFormat)
            // {
            //     // case "gif":
            //     // {
            //     //     imageWriter = new GifWriter();
            //     //     break;
            //     // }
            //     // case "png":
            //     // {
            //     //     imageWriter = new PngWriter();
            //     //     break;
            //     // }
            //     case "ppm":
            //     {
            //         imageWriter = new PPMWriter();
            //         break;
            //     }
            // }

            string path = "C:\\Users\\mmaks\\Desktop\\sun";
            Converter converter = new Converter(imageReader, imageWriter);
            converter.Convert(path);
            Console.WriteLine("Image converted");

        }
    }
}
