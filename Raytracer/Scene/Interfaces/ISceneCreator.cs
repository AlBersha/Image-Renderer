using System.Collections.Generic;
using Raytracer.ObjectProvider;

namespace Raytracer.Scene.Interfaces
{
    public interface ISceneCreator
    {
        public IParamsProvider ParamsProvider { get; }
        public IObjectFromFileProvider ObjectProvider { get; set; }
        public void CreateScreen(string filePath);
        public List<ObjectModel> GetObjects(string filePath);
        public float SetXScreenCoordinate(float x);
        public float SetYScreenCoordinate(float y);

    }
}