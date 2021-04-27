using System;
using System.IO;
using System.Numerics;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Data.VertexData;
using Raytracer;
using ObjLoader.Loader.Loaders;
using Raytracer.Optimisation;

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

            var width = 1000;
            var height = 800;
            
            var path = "C:\\Users\\obers\\KPI\\graphics\\cow.obj";
            string outputPath = "C:\\Users\\obers\\KPI\\graphics\\cow.ppm";
            
            var object3D = ObjReader.ReadObjFile(path);
        
            var translationM = new Matrix4x4 
            (
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
            var scaleM = new Matrix4x4 
            (
                2f, 0, 0, 0,
                0, 2f, 0, 0,
                0, 0, 2f, 0,
                0, 0, 0, 1
            );
            var rotationX = new Matrix4x4 
            (
                1, 0, 0, 0,
                0, 0, 1, 0,
                0, -1, 0, 0,
                0, 0, 0, 1
            );
            var rotationY = new Matrix4x4 
            (
                -1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, -1, 0,
                0, 0, 0, 1
            );
            var rotationZ = new Matrix4x4 
            (
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
            
            Extension.Transform(ref object3D,rotationZ, rotationY,rotationX, scaleM,translationM);
            var min = new Vector3();
            var max = new Vector3();

            Extension.BoundingBoxCoordinates(object3D, ref min, ref max);
            var faces = Extension.GetTrianglesList(object3D);

            var octree = new Octree();
            octree.CreateTree(min, max, faces);
            var tracer = new Tracer();
            var pixelMatrix = tracer.Trace(width,height, octree);
            
            IImage image = new PPM(width, height, pixelMatrix);
            var ppmWriter = new PPMWriter();
            ppmWriter.WriteImage(image, outputPath);
        }
    }
}
