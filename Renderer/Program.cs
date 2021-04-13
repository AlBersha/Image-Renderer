using System;
using System.IO;
using ConverterBase.GeomHelper;
using Raytracer;

namespace Renderer
{
    using System.Collections.Generic;
    using BMPReader;
    using ConverterBase;
    using ConverterBase.Readers;
    using ConverterBase.Writers;
    using GifFormat;
    using PNGFormat;
    using PPMFormat;

    class Program
    {
        static void Main(string[] args)
        {
            // ICommandProcessor commandProcessor = new CommandConsoleProcessor();
            // commandProcessor.ProcessCommand(args);
            //
            // List<string> availableForReading = new List<string>()
            // {
            //     "ppm", "bmp", "gif", "png"
            // };
            //
            // List<string> availableForWriting = new List<string>()
            // {
            //     "ppm", "bmp"
            // };
            //
            // FormatValidator formatValidator = new FormatValidator(availableForReading, availableForWriting);
            // if (!formatValidator.ValidateSourceFileFormat(commandProcessor.SourceFormat))
            // {
            //     Console.Write($"You are trying to open {commandProcessor.SourceFormat} file, but only ");
            //     foreach (var format in availableForReading)
            //     {
            //         Console.Write('.' + format + " ");
            //     }
            //     Console.Write("files are supported for reading");
            // }
            //
            // if (!formatValidator.ValidateGoalFileFormat(commandProcessor.GoalFormat))
            // {
            //     Console.Write($"You are trying to write {commandProcessor.GoalFormat} file, but only ");
            //     foreach (var format in availableForWriting)
            //     {
            //         Console.Write('.' + format + " ");
            //     }
            //     Console.Write("files are supported for writing");
            // }
            //
            //
            // IImageReader imageReader = null;
            // IImageWriter imageWriter = null;
            //
            // switch (commandProcessor.SourceFormat)
            // {
            //     case "ppm":
            //         imageReader = new PPMReader();
            //         break;
            //     case "bmp":
            //         imageReader = new BMPReader();
            //         break;
            //     case "gif":
            //         imageReader = new GifReader();
            //         break;
            //     case "png":
            //         imageReader = new PNGReader();
            //         break;
            //     
            // }
            //
            // switch (commandProcessor.GoalFormat)
            // {
            //     case "ppm":
            //         imageWriter = new PPMWriter();
            //         break;
            //     case "bmp":
            //         imageWriter = new BMPWriter();
            //         break;
            // }
            //
            // Converter converter = new Converter(imageReader, imageWriter);
            // converter.Convert(commandProcessor.SourceFile, commandProcessor.OutputFile);
            // Console.WriteLine("Image converted");

            int width = 30;
            int height = 30;
            
            Point3[,] matrix = new Point3[width, height];
            matrix = Tracer.Trace();

            // for (int i = 0; i < width; i++)
            // {
            //     for (int j = 0; j < height; j++)
            //     {
            //         if (matrix[i, j])
            //         {
            //             Console.Write("*");
            //         }
            //         else
            //         {
            //             Console.Write(".");
            //         }
            //     }
            //     Console.WriteLine();
            // }

            string outputPath = "C:\\Users\\obers\\Downloads\\result.ppm";
            
            List<string> fileData = new List<string>();
            fileData.Add("P3");
            
            fileData.Add(width.ToString() + ' ' + height.ToString());
            fileData.Add("255");
            
            for (int i = 0; i < height; i++)
            {
                string row = "";
                for (int j = 0; j < width; j++)
                {
                    row += matrix[i,j].X + " " + matrix[i,j].Y + " " + matrix[i,j].Z + "   ";
                }
                fileData.Add(row);
            }
            
            File.WriteAllLines(outputPath, fileData);
            

        }
    }
}
