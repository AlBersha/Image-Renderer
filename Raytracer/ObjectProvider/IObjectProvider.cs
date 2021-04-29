namespace Raytracer.ObjectProvider
{
    public interface IObjectFromFileProvider
    {
        public ObjectModel ParseObjectToObjectModel(string pathToFile);
    }
}