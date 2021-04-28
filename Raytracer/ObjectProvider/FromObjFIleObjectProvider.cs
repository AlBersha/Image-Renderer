using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Security.Principal;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Loaders;

namespace Raytracer.ObjectProvider
{
    public class FromObjFIleObjectProvider: IObjectFromFileProvider
    {
        public ObjectModel ObjectModel { get; set; } = new ObjectModel();
        private LoadResult object3D = new LoadResult();
        
        public ObjectModel ParseObjectToObjectModel(string pathToFile)
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            
            var fileStream = File.OpenRead(pathToFile);
            object3D = objLoader.Load(fileStream);

            LoadResultToObjectModel();
            return ObjectModel;
        }

        private void LoadResultToObjectModel()
        {
            GetVertices();
            GetNormals();
            GetFaces();
            GetTextures();
        }

        private void GetVertices()
        {
            foreach (var vertex in object3D.Vertices)
            {
                ObjectModel.Vertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
            }
        }

        private void GetNormals()
        {
            foreach (var normal in object3D.Normals)
            {
                ObjectModel.VerticesNormals.Add(new Vector3(normal.X, normal.Y, normal.Z));
            }
        }
        
        private void GetFaces()
        {
            var faces = new List<Triangle>();
            foreach (var face in object3D.Groups[0].Faces)
            {
                var X = object3D.Vertices[face[0].VertexIndex - 1].X;
                var Y = object3D.Vertices[face[0].VertexIndex - 1].Y;
                var Z = object3D.Vertices[face[0].VertexIndex - 1].Z;
                var A = new Vector3(X, Y, Z);

                X = object3D.Vertices[face[1].VertexIndex - 1].X;
                Y = object3D.Vertices[face[1].VertexIndex - 1].Y;
                Z = object3D.Vertices[face[1].VertexIndex - 1].Z;
                var B = new Vector3(X, Y, Z);

                X = object3D.Vertices[face[2].VertexIndex - 1].X;
                Y = object3D.Vertices[face[2].VertexIndex - 1].Y;
                Z = object3D.Vertices[face[2].VertexIndex - 1].Z;
                var C = new Vector3(X, Y, Z);

                faces.Add(new Triangle(A, B, C));
            }

            ObjectModel.Faces = faces;
        }

        private void GetTextures()
        {
            foreach (var texture in object3D.Textures)
            {
                ObjectModel.Textures.Add(texture);
            }
        }
    }
}