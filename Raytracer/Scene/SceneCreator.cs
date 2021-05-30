using System.Collections.Generic;
using Raytracer.ObjectProvider;
using Raytracer.Scene.Interfaces;

namespace Raytracer.Scene
{
    public class SceneCreator: ISceneCreator
    {
        private readonly IScreenProvider _screen;
        public IParamsProvider ParamsProvider { get; }
        public IObjectFromFileProvider ObjectProvider { get; set; }
        public SceneCreator(IParamsProvider paramsProvider, IScreenProvider screen, IObjectFromFileProvider objectProvider)
            => (_screen, ParamsProvider, ObjectProvider) = (screen, paramsProvider, objectProvider);

        public void CreateScreen(string filePath)
        {
            _screen.SetScreenProperties(ParamsProvider);
        }

        public List<ObjectModel> GetObjects(string filePath)
        {
             return new List<ObjectModel> {ObjectProvider.ParseObject(filePath)};
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