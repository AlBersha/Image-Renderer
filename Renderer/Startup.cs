
using System;
using System.Numerics;
using ConsoleProcessor;
using ConverterBase;
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

        // todo the class supposed to be all required config setter

        public void ConfigureExecutor(string[] args)
        {
            // 1. console management
            // such params would be read from console
            // var pathToFile = "C:\\Users\\obers\\KPI\\graphics\\cow.obj";
            // var outputPath = "C:\\Users\\obers\\KPI\\graphics\\out.ppm";

            _commandProcessor.ProcessCommand(args);
            
            // 2. get object
            var object3D = _object.ParseObjectToObjectModel(_commandProcessor.SourceFile); // todo separate path to file 
            
            // 3. transform object
            
            Transformation.RotateZ();
            Transformation.RotateX();
            Transformation.RotateY();
            Transformation.Scale();
            Transformation.Translate();
            Transformation.Transform(ref object3D);

            // 4. build tree
            _treeProvider.CreateTree(object3D);
            
            // 5. create screen
            _sceneCreator.CreateScreen();

            // 6. execute tracing
            var pixels = _tracer.Trace(_sceneCreator, _treeProvider);

            // 7. write to file 
            var image = new PPM((int)_sceneCreator.ParamsProvider.ImageWidth, (int)_sceneCreator.ParamsProvider.ImageHeight, pixels);
            var ppmWriter = new PPMWriter();
            ppmWriter.WriteImage(image, _commandProcessor.OutputFile);
        }
    }
}