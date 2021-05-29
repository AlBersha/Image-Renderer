using System.IO;
using ObjLoader.Loader.Loaders;

namespace Raytracer.ObjectProvider
{
    public class FromObjFIleObjectProvider: IObjectFromFileProvider
    {
        private ObjectModel ObjectModel { get; } = new ObjectModel();
        
        public ObjectModel ParseObject(string pathToFile)
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            
            var fileStream = File.OpenRead(pathToFile);
            var object3D = objLoader.Load(fileStream);

            return LoadResultToObjectModel.ToObjectModel(object3D);
        }
    }
}