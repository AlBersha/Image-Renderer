using System.Numerics;

namespace Raytracer.Scene
{
    public interface ISceneCreator
    {
        public void CreateScreen();
        public float SetXScreenCoordinate(float x);
        public float SetYScreenCoordinate(float y);

    }
}