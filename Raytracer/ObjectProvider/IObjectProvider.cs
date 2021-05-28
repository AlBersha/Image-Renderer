namespace Raytracer.ObjectProvider
{
    public interface IObjectFromFileProvider
    {
        public ObjectModel ParseObject(string pathToFile);
    }
}