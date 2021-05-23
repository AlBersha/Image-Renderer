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
        private ObjectModel ObjectModel { get; } = new ObjectModel();
        
        public ObjectModel ParseObjectToObjectModel(string pathToFile)
        {
            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            
            var fileStream = File.OpenRead(pathToFile);
            var object3D = objLoader.Load(fileStream);

            LoadResultToObjectModel(object3D);
            return ObjectModel;
        }

        private void LoadResultToObjectModel(LoadResult loadResult)
        {
            GetVertices(loadResult);
            GetNormals(loadResult);
            GetFaces(loadResult);
            GetTextures(loadResult);
        }

        private void GetVertices(LoadResult loadResult)
        {
            foreach (var vertex in loadResult.Vertices)
            {
                ObjectModel.Vertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
            }
        }

        private void GetNormals(LoadResult loadResult)
        {
            foreach (var normal in loadResult.Normals)
            {
                ObjectModel.VerticesNormals.Add(new Vector3(normal.X, normal.Y, normal.Z));
            }
        }
        
        private void GetFaces(LoadResult loadResult)
        {
            var faces = new List<Triangle>();
            foreach (var face in loadResult.Groups[0].Faces)
            {
                var X = loadResult.Vertices[face[0].VertexIndex - 1].X;
                var Y = loadResult.Vertices[face[0].VertexIndex - 1].Y;
                var Z = loadResult.Vertices[face[0].VertexIndex - 1].Z;
                var A = new Vector3(X, Y, Z);

                X = loadResult.Vertices[face[1].VertexIndex - 1].X;
                Y = loadResult.Vertices[face[1].VertexIndex - 1].Y;
                Z = loadResult.Vertices[face[1].VertexIndex - 1].Z;
                var B = new Vector3(X, Y, Z);

                X = loadResult.Vertices[face[2].VertexIndex - 1].X;
                Y = loadResult.Vertices[face[2].VertexIndex - 1].Y;
                Z = loadResult.Vertices[face[2].VertexIndex - 1].Z;
                var C = new Vector3(X, Y, Z);

                var Xn = loadResult.Normals[face[0].NormalIndex - 1].X;
                var Yn = loadResult.Normals[face[0].NormalIndex - 1].Y;
                var Zn = loadResult.Normals[face[0].NormalIndex - 1].Z;
                var An = new Vector3(Xn, Yn, Zn);
                
                Xn = loadResult.Normals[face[1].NormalIndex - 1].X;
                Yn = loadResult.Normals[face[1].NormalIndex - 1].Y;
                Zn = loadResult.Normals[face[1].NormalIndex - 1].Z;
                var Bn = new Vector3(Xn, Yn, Zn);

                Xn = loadResult.Normals[face[2].NormalIndex - 1].X;
                Yn = loadResult.Normals[face[2].NormalIndex - 1].Y;
                Zn = loadResult.Normals[face[2].NormalIndex - 1].Z;
                var Cn = new Vector3(Xn, Yn, Zn);


                faces.Add(new Triangle(A, B, C, An, Bn, Cn));
            }

            ObjectModel.Faces = faces;
        }

        private void GetTextures(LoadResult loadResult)
        {
            foreach (var texture in loadResult.Textures)
            {
                ObjectModel.Textures.Add(texture);
            }
        }
    }
}