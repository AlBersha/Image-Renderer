using System.Numerics;

namespace Raytracer.Scene
{
    public class StaticCameraProvider: ICameraProvider
    {
        public Vector3 SetCamera()
        {
            return new Vector3(0, 0, 2);
        }
    }
}