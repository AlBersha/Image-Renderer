using System.Numerics;

namespace Raytracer.Scene
{
    public class LightProvider: ILightProvider
    {
        public Vector3 SetIlluminant()
        {
            return new Vector3(-2, 2, 0);
        }
    }
}