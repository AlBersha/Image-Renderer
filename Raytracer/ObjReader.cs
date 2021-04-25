using System.IO;
using ObjLoader.Loader.Loaders;

namespace Raytracer
{
    public static class ObjReader
    {
        public static LoadResult ReadObjFile(string path)
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            
            var fileStream = File.OpenRead(path);
            var result = objLoader.Load(fileStream);
            
            return result;
        }
    }
}