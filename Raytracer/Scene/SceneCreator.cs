using System.Numerics;

namespace Raytracer.Scene
{
    public class SceneCreator: ISceneCreator
    {
        private IScreenProvider _screen;
        private IParamsProvider _paramsProvider;

        public SceneCreator(IParamsProvider paramsProvider, IScreenProvider screen)
        {
            _paramsProvider = paramsProvider;
            _screen = screen;
        }

        public void CreateScreen()
        {
            _screen.SetScreenProperties(_paramsProvider);
        }

        public float SetXScreenCoordinate(float x)
        {
            return _screen.XToScreenCoordinates(x);
        }

        public float SetYScreenCoordinate(float y)
        {
            return _screen.YToScreenCoordinates(y);
        }

    }

}