
using System;
using BMPReader;
using ConsoleProcessor;
using ConverterBase;
using ConverterBase.Readers;
using ConverterBase.Writers;
using GifFormat;
using PNGFormat;
using PPMFormat;
using Raytracer.ObjectProvider;
using Raytracer.Optimisation;
using Raytracer.Scene;
using Raytracer.Scene.Interfaces;
using Raytracer.Tracing;
using Raytracer.Transformation;

namespace Renderer
{
    public class Startup
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly ISceneCreator _sceneCreator;
        private readonly IObjectFromFileProvider _object;
        private readonly ITreeProvider _treeProvider;
        private readonly ITracer _tracer;

        public Startup(ICommandProcessor commandProcessor, ISceneCreator sceneCreator,
            IObjectFromFileProvider objectFromFileProvider,
            ITreeProvider treeProvider, ITracer tracer) =>
            (_commandProcessor, _sceneCreator, _object, _treeProvider, _tracer) = (commandProcessor, sceneCreator,
                objectFromFileProvider,
                treeProvider, tracer);

        public void ConfigureExecutor(string[] args)
        {
            _commandProcessor.ProcessCommand(args);

            if (_commandProcessor.SourceFormat != "obj" && _commandProcessor.SourceFormat != "cowscene")
            {
                ExecuteConversion();
            }
            else
            {
                ExecuteRaytracing();
            }
        }

        private void ExecuteConversion()
        {
            
            IImageReader imageReader = null;
            IImageWriter imageWriter = null;
            
            switch (_commandProcessor.SourceFormat)
            {
                case "ppm":
                    imageReader = new PPMReader();
                    break;
                case "bmp":
                    imageReader = new BMPReader.BMPReader();
                    break;
                case "gif":
                    imageReader = new GifReader();
                    break;
                case "png":
                    imageReader = new PNGReader();
                    break;
                default:
                    Console.WriteLine($"You are trying to open {_commandProcessor.SourceFormat} file, but it is not implemented yet.");
                    break;
            }
            
            switch (_commandProcessor.GoalFormat)
            {
                case "ppm":
                    imageWriter = new PPMWriter();
                    break;
                case "bmp":
                    imageWriter = new BMPWriter();
                    break;
                default:
                    Console.WriteLine($"You are trying to write {_commandProcessor.GoalFormat} file, but it is not implemented yet.");
                    break;
            }
            
            Converter converter = new Converter(imageReader, imageWriter);
            converter.Convert(_commandProcessor.SourceFile, _commandProcessor.OutputFile);
            Console.WriteLine("Image converted");
        }

        private void ExecuteRaytracing()
        {
            var object3D = _object.ParseObject(_commandProcessor.SourceFile); 
            
            // Transformation.RotateZ();
            Transformation.RotateX();
            // Transformation.RotateY();
            Transformation.Scale();
            Transformation.Translate();
            Transformation.Transform(ref object3D);

            _treeProvider.CreateTree(object3D);
            
            _sceneCreator.CreateScreen();

            var pixels = _tracer.Trace(_sceneCreator, _treeProvider);

            var image = new PPM((int)_sceneCreator.ParamsProvider.ImageWidth, (int)_sceneCreator.ParamsProvider.ImageHeight, pixels);
            var ppmWriter = new PPMWriter();
            ppmWriter.WriteImage(image, _commandProcessor.OutputFile);
        }
    }
}