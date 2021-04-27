using System.Numerics;

namespace Raytracer.Scene
{
    public class SceneCreator
    {
        private ICameraProvider _camera;
        private IScreenProvider _screen;
        private ILightProvider _light;
        private IRaysProvider _rays;

        public SceneCreator(ICameraProvider camera, IScreenProvider screen, ILightProvider light, IRaysProvider rays)
        {
            _camera = camera;
            _screen = screen;
            _light = light;
            _rays = rays;
        }
        
        //TODO implement methods to provide scene
        
        public void SetSceneCore()
        {
            var camera = _camera.SetCamera();
            var light = _light.SetIlluminant();
        }
        
        


        public float SetXCoordinate(float x)
        {
            return _screen.XToScreenCoordinates(x);
        }

        public float SetYCoordinate(float y)
        {
            return _screen.YToScreenCoordinates(y);
        }

    }

    public class SceneModel
    {
        public Vector3 Camera { get; set; }
        public Vector3 Light { get; set; }
        
    }
}