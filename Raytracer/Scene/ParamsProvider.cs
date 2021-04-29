using System.Numerics;

namespace Raytracer.Scene
{
    public class ParamsProvider: IParamsProvider
    {
        public float ImageWidth { get; set; } = 500;
        public float ImageHeight { get; set; } = 500;
        public int Fov { get; set; } = 90;
        public float ScreenZ { get; set; } = 1f;
        public Vector3 Camera { get; set; } = new Vector3(0, 0, 2);
        public Vector3 LightPosition { get; set; } = new Vector3(0, 2, 2);
    }
}