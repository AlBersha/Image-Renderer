using Raytracer.Scene.Interfaces;

namespace Raytracer.Scene
{
    public class SceneCreator: ISceneCreator
    {
        private IScreenProvider _screen;
        public IParamsProvider ParamsProvider { get; }

        public SceneCreator(IParamsProvider paramsProvider, IScreenProvider screen)
        {
            ParamsProvider = paramsProvider;
            _screen = screen;
        }

        public void CreateScreen()
        {
            _screen.SetScreenProperties(ParamsProvider);
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