using System.Numerics;

namespace Raytracer.Scene
{
    public interface IParamsProvider
    {
        public float ImageWidth { get; set; }
        public float ImageHeight { get; set; }
        public int Fov { get; set; }
        public float ScreenZ { get; set; }
        public Vector3 Camera { get; set; }
        public Vector3 LightPosition { get; set; }
        
        
    }
}