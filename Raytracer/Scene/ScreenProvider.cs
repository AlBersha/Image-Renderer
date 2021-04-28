using System;

namespace Raytracer.Scene
{
    public class ScreenProvider: IScreenProvider
    {
        
        private float ImageAspectRatio => ImageWidth / ImageHeight;
        private float ImageWidth { get; set; }
        private float ImageHeight { get; set; }
        private int Fov { get; set; }
        private float ScreenZ { get; set; }

        // public ScreenProvider()
        // {
        //     ImageWidth = 600;
        //     ImageHeight = 800;
        //     Fov = 90;
        // }

        public void SetScreenProperties(IParamsProvider _params)
        {
            ImageWidth = _params.ImageWidth;
            ImageHeight = _params.ImageHeight;
            Fov = _params.Fov;
            ScreenZ = _params.ScreenZ;
        }

        public void GetScreenProperties()
        {
            throw new NotImplementedException();
        }

        public float XToScreenCoordinates(float x)
        { 
            return (float) ((2 * ((x + 0.5) / ImageWidth) - 1) * Math.Tan(Fov / 2f * Math.PI / 180) * ImageAspectRatio);
        }

        public float YToScreenCoordinates(float y)
        {
            return (float) ((1 - 2 * ((y + 0.5) / ImageHeight)) * Math.Tan(Fov / 2f * Math.PI / 180));
        }
    }
}