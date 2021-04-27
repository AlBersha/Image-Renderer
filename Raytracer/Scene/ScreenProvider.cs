using System;

namespace Raytracer.Scene
{
    public class ScreenProvider: IScreenProvider
    {
        private static float ImageWidth { get; set; }
        private static float ImageHeight { get; set; }
        private float ImageAspectRatio => ImageWidth / ImageHeight;
        private float Fov { get; set; }
        
        public ScreenProvider()
        {
            ImageWidth = 600;
            ImageHeight = 800;
            Fov = 90;
        }

        public ScreenProvider(float width, float height,  float fov = 90)
        {
            ImageWidth = width;
            ImageHeight = height;
            Fov = fov;
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