namespace Raytracer.Scene.Interfaces
{
    public interface ISceneCreator
    {
        public IParamsProvider ParamsProvider { get; }
        public void CreateScreen();
        public float SetXScreenCoordinate(float x);
        public float SetYScreenCoordinate(float y);

    }
}