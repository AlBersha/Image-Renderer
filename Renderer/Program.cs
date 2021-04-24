﻿using System;
using System.IO;
using System.Numerics;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Data.VertexData;
using Raytracer;
using ObjLoader.Loader.Loaders;

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

            var width = 1500;
            var height = 1500;
            
            var path = "C:\\Users\\mmaks\\Desktop\\cow.obj";
            string outputPath = "C:\\Users\\mmaks\\Desktop\\100x100.ppm";
            
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
            
            var transformM = rotationY * rotationX * scaleM * translationM;
            
            var xMin = float.MaxValue;
            var yMin = float.MaxValue;
            var zMin = float.MaxValue;
            var xMax = float.MinValue;
            var yMax = float.MinValue;
            var zMax = float.MinValue;
            
            for (var i = 0; i < object3D.Vertices.Count; i++)
            {
                var v3 = new Vector3(object3D.Vertices[i].X, object3D.Vertices[i].Y, object3D.Vertices[i].Z);
            
                var v4 = new Vector4(v3.X, v3.Y, v3.Z, 1);
                var res = transformM.MultiplyBy(v4);
            
                object3D.Vertices[i] = new Vertex(res.X, res.Y, res.Z);
            
                xMin = Math.Min(xMin, object3D.Vertices[i].X);
                yMin = Math.Min(yMin, object3D.Vertices[i].Y);
                zMin = Math.Min(zMin, object3D.Vertices[i].Z);
                xMax = Math.Max(xMax, object3D.Vertices[i].X);
                yMax = Math.Max(yMax, object3D.Vertices[i].Y);
                zMax = Math.Max(zMax, object3D.Vertices[i].Z);
            }
            
            var min = new Vector3(xMin, yMin, zMin);
            var max = new Vector3(xMax, yMax, zMax);
            
            var pixelMatrix = Tracer.Trace(width,height, object3D, min,max);
            
            IImage image = new PPM(width, height, pixelMatrix);
            var ppmWriter = new PPMWriter();
            ppmWriter.WriteImage(image, outputPath);

            //----------
        }
    }
}
