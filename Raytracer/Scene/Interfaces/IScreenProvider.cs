using System;
using System.Security.Principal;

namespace Raytracer.Scene
{
    public interface IScreenProvider
    {
        public void SetScreenProperties(IParamsProvider paramsProvider);
        public void GetScreenProperties();
        public float XToScreenCoordinates(float x);
        public float YToScreenCoordinates(float y);
    }
}