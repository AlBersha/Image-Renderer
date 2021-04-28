namespace Raytracer.ObjectProvider
{
    public interface IObjectFromFileProvider
    {
        public ObjectModel ObjectModel { get; set; }

        public ObjectModel ParseObjectToObjectModel(string pathToFile);
    }
}