using System.IO;
using ObjLoader.Loader.Loaders;
using SceneFormat;

namespace Raytracer.ObjectProvider
{
    public class CustomSceneObjectProvider: IObjectFromFileProvider
    {
        private ObjectModel ObjectModel { get; } = new ObjectModel();
        
        private static readonly ISceneIO _sceneIO = new SceneIO();

        public ObjectModel ParseObject(string pathToFile)
        {
            var sceneData = _sceneIO.Read(pathToFile);
            var obj = sceneData.SceneObjects[0];
            
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            
            var fileStream = File.OpenRead(obj.MeshedObject.Reference);
            var object3D = objLoader.Load(fileStream);

            LoadResultToObjectModel.GetVertices(object3D);
            LoadResultToObjectModel.GetNormals(object3D);
            LoadResultToObjectModel.GetFaces(object3D);
            LoadResultToObjectModel.GetTextures(object3D);
            
            return ObjectModel;
        }
        
    }
}