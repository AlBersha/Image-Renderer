
using System;
using System.Numerics;
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
        private readonly ISceneCreator _sceneCreator;
        private readonly IObjectFromFileProvider _object;
        private readonly ITreeProvider _treeProvider;
        private readonly ITracer _tracer;

        public Startup(ISceneCreator sceneCreator, IObjectFromFileProvider objectFromFileProvider,
            ITreeProvider treeProvider, ITracer tracer) =>
            (_sceneCreator, _object, _treeProvider, _tracer) = (sceneCreator, objectFromFileProvider,
                treeProvider, tracer);

        // todo the class supposed to be all required config setter

        public void ConfigureExecutor()
        {
            // 1. console management
            // such params would be read from console
            var pathToFile = "C:\\Users\\obers\\KPI\\graphics\\cow.obj";
            var outputPath = "C:\\Users\\obers\\KPI\\graphics\\cow.ppm";
            
            // 2. get object
            var object3D = _object.ParseObjectToObjectModel(pathToFile); // todo separate path to file 
            
            // 3. transform object
            // Transformation.RotateX();
            // Transformation.RotateY();
            // Transformation.RotateZ();
            // Transformation.Scale();
            // Transformation.Transform(object3D);
            
            
            // _transformation.Transform(ref object3D);
            
            // 4. build tree
            _treeProvider.CreateTree(Transformation.Transform(object3D));
            
            // 5. create screen
            _sceneCreator.CreateScreen();

            // 6. execute tracing
            var pixels = _tracer.Trace(_sceneCreator, _treeProvider);

            var image = new PPM(500, 500, pixels);
            var ppmWriter = new PPMWriter();
            ppmWriter.WriteImage(image, outputPath);
        }
    }
}