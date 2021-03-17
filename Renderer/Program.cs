using System;

namespace Renderer
{
    using ConverterBase;
    using ConverterBase.Readers;
    using ConverterBase.Writers;
    using GifFormat;

    class Program
    {
        static void Main(string[] args)
        {
            var sourceFile = "";
            var goalFormat = "";
            var outputFile = "";
            
            foreach (var arg in args)
            {
                if (arg.StartsWith("--source="))
                {
                    sourceFile = arg.Substring(arg.IndexOf('=') + 1);
                };
                
                if (arg.StartsWith("--goal-format"))
                {
                    goalFormat = arg.Substring(arg.IndexOf('=') + 1);
                }

                if (arg.StartsWith("--output"))
                {
                    outputFile = arg.Substring(arg.IndexOf('=') + 1);
                }
            }

            if (sourceFile == "")
            {
                Console.WriteLine("Argument --source is either entered incorrectly or is missing");
            }

            if (goalFormat == "")
            {
                Console.WriteLine("Argument --source is either entered incorrectly or is missing");
            }

            if (outputFile == "")
            {
                outputFile = sourceFile;
            }

            // IImageReader imageReader;
            // IImageWriter imageWriter;
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
            //     case "gif":
            //     {
            //         imageWriter = new GifWriter();
            //         break;
            //     }
            //     case "png":
            //     {
            //         imageWriter = new PngWriter();
            //         break;
            //     }
            //         
            // }
            //
            // Converter converter = new Converter(imageReader, imageWriter);
        }
    }
}