using System;
using System.Collections.Generic;
using Raytracer.ObjectProvider;
using Raytracer.Scene.Interfaces;
using SceneFormat;
using Vector3 = System.Numerics.Vector3;

namespace Raytracer.Scene
{
    public class FromFileSceneCreator: ISceneCreator
    {
        private readonly IScreenProvider _screen;
        public IParamsProvider ParamsProvider { get; }
        private SceneFormat.Scene SceneData { get; set; }
        public IObjectFromFileProvider ObjectProvider { get; set; }
        private ISceneIO SceneIo { get; set; } = new SceneIO();

        public FromFileSceneCreator(IParamsProvider paramsProvider, IScreenProvider screen,
            IObjectFromFileProvider objectProvider)
        {
            ParamsProvider = paramsProvider;
            _screen = screen;
            ObjectProvider = objectProvider;
        }
        
        public void CreateScreen(string filePath)
        {
            SceneData = SceneIo.Read(filePath);

            try
            {
                var c = SceneData.Cameras[0];
                if (c !=  null)
                {
                    ParamsProvider.Fov = (int) c.Perspective.Fov;
                    ParamsProvider.Camera = new Vector3((float)c.Transform.Position.X, (float)c.Transform.Position.Y, (float)c.Transform.Position.Z);
                }
            
                var l = SceneData?.Lights[0].Transform.Position;
                if (l != null)
                {
                    ParamsProvider.LightPosition = new Vector3((float)l.X, (float)l.Y, (float)l.Z);
                }
            }
            catch (Exception )
            {
                Console.WriteLine("Some scene parameters were not specified in the file. The default parameters will be used");
            }
            
            _screen.SetScreenProperties(ParamsProvider);
        }

        public List<ObjectModel> GetObjects(string filePath)
        {
            // SceneData = SceneIo.Read(filePath);
            var objects = new List<ObjectModel>();
            foreach (var sceneObject in SceneData.SceneObjects)
            {
                var obj = ProcessObject(sceneObject);
                if (obj != null)
                {
                    objects.Add(obj);
                }
            }
            return objects;
        }

        private ObjectModel ProcessObject(SceneObject sceneObject)
        {
            var objectModel = new ObjectModel();
            try
            {
                var objectPath = sceneObject.MeshedObject.Reference;
                objectModel = ObjectProvider.ParseObject(objectPath);

                var rotate = sceneObject.Transform.Rotation;
                var scale = sceneObject.Transform.Scale;
                var translate = sceneObject.Transform.Position;

                var transformation = new Transformation.Transformation();
                transformation.RotateX((float) rotate.X);
                transformation.RotateY((float) rotate.Y);
                transformation.RotateZ((float) rotate.Z);
                transformation.Scale(new Vector3((float) scale.X, (float) scale.Y, (float) scale.Z));
                transformation.Translate(new Vector3((float) translate.X, (float) translate.Y, (float) translate.Z));
                transformation.Transform(ref objectModel);

                
            }
            catch (Exception e)
            {
                // ignored
            }

            return objectModel;
        }

        public float SetXScreenCoordinate(float x)
        {
            return _screen.XToScreenCoordinates(x);
        }

        public float SetYScreenCoordinate(float y)
        {
            return _screen.YToScreenCoordinates(y);
        }
    }
}