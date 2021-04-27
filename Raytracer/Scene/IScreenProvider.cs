using System;

namespace Raytracer.Scene
{
    public interface IScreenProvider
    {
        public float XToScreenCoordinates(float x);
        public float YToScreenCoordinates(float y);
    }
}