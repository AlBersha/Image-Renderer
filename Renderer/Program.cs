﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raytracer.ObjectProvider;
using Raytracer.Optimisation;
using Raytracer.Scene;
using Raytracer.Tracing;
using Raytracer.Transformation;

namespace Renderer
{
    internal static class Program
    {
        private static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            
            ExemplifyScoping(host.Services);
            

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

            // var width = 1000;
            // var height = 800;
            //
            // var path = "C:\\Users\\obers\\KPI\\graphics\\cow.obj";
            // string outputPath = "C:\\Users\\obers\\KPI\\graphics\\cow.ppm";
            //
            // // var object3D = ObjReader.ReadObjFile(path);
            //
            // var translationM = new Matrix4x4 
            // (
            //     1, 0, 0, 0,
            //     0, 1, 0, 0,
            //     0, 0, 1, 0,
            //     0, 0, 0, 1
            // );
            // var scaleM = new Matrix4x4 
            // (
            //     2f, 0, 0, 0,
            //     0, 2f, 0, 0,
            //     0, 0, 2f, 0,
            //     0, 0, 0, 1
            // );
            // var rotationX = new Matrix4x4 
            // (
            //     1, 0, 0, 0,
            //     0, 0, 1, 0,
            //     0, -1, 0, 0,
            //     0, 0, 0, 1
            // );
            // var rotationY = new Matrix4x4 
            // (
            //     -1, 0, 0, 0,
            //     0, 1, 0, 0,
            //     0, 0, -1, 0,
            //     0, 0, 0, 1
            // );
            // var rotationZ = new Matrix4x4 
            // (
            //     1, 0, 0, 0,
            //     0, 1, 0, 0,
            //     0, 0, 1, 0,
            //     0, 0, 0, 1
            // );

            // Extension.Transform(ref object3D,rotationZ, rotationY,rotationX, scaleM,translationM);
            // var min = new Vector3();
            // var max = new Vector3();
            //
            // Extension.BoundingBoxCoordinates(object3D, ref min, ref max);
            // var faces = Extension.GetTrianglesList(object3D);
            //
            // var octree = new Octree();
            // octree.CreateTree(min, max, faces);
            // var tracer = new Tracer();
            // var pixelMatrix = tracer.Trace(width,height, octree);

            // IImage image = new PPM(width, height, pixelMatrix);
            // var ppmWriter = new PPMWriter();
            // ppmWriter.WriteImage(image, outputPath);
            
            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.RegisterTree();
                    services.RegisterScene();
                    services.RegisterObjectProvider();
                    services.RegisterTracer();
                    services.AddTransient<Startup>();
                });

        private static void ExemplifyScoping(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var startup = provider.GetService<Startup>();
            startup?.ConfigureExecutor();
        }
    }
}
