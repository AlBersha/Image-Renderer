
using System;
using System.Numerics;
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
        private ISceneCreator _sceneCreator;
        private IObjectFromFileProvider _object;
        private ITransformation _transformation;
        private ITreeProvider _treeProvider;
        private ITracer _tracer;

        public Startup(ISceneCreator sceneCreator, IObjectFromFileProvider objectFromFileProvider, ITransformation transformation, ITreeProvider treeProvider, ITracer tracer)
        {
            _sceneCreator = sceneCreator;
            _object = objectFromFileProvider;
            _transformation = transformation;
            _treeProvider = treeProvider;
            _tracer = tracer;
        }
        
        // todo the class supposed to be all required config setter

        public void ConfigureExecutor()
        {
            // 1. console management
            
            
            // 2. get object
            var object3D = _object.ParseObjectToObjectModel("path to file"); // todo separate path to file 
            
            // 3. transform object
            _transformation.RotateZ();
            _transformation.RotateY();
            _transformation.RotateX();
            _transformation.Scale();

            _transformation.Transform(ref object3D);
            
            // 4. build tree
            _treeProvider.CreateTree(object3D);
            
            // 5. create screen
            _sceneCreator.CreateScreen();

            // 6. execute tracing
            var pixels = _tracer.Trace(_sceneCreator, _treeProvider);

        }
    }
}