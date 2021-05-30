using System.Collections.Generic;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Loaders;
using SceneFormat;

namespace Raytracer.ObjectProvider
{
    public static class LoadResultToObjectModel
    {
        private static ObjectModel ObjectModel { get; set; }

        public static ObjectModel ToObjectModel(LoadResult loadResult)
        {
            ObjectModel = new ObjectModel();
            GetVertices(loadResult);
            GetNormals(loadResult);
            GetFaces(loadResult);
            GetTextures(loadResult);

            return ObjectModel;
        }
        
        public static void GetVertices(LoadResult loadResult)
        {
            foreach (var vertex in loadResult.Vertices)
            {
                ObjectModel.Vertices.Add(new System.Numerics.Vector3(vertex.X, vertex.Y, vertex.Z));
            }
        }

        public static void GetNormals(LoadResult loadResult)
        {
            foreach (var normal in loadResult.Normals)
            {
                ObjectModel.VerticesNormals.Add(new System.Numerics.Vector3(normal.X, normal.Y, normal.Z));
            }
        }
        
        public static void GetFaces(LoadResult loadResult)
        {
            var faces = new List<Triangle>();
            foreach (var face in loadResult.Groups[0].Faces)
            {
                var X = loadResult.Vertices[face[0].VertexIndex - 1].X;
                var Y = loadResult.Vertices[face[0].VertexIndex - 1].Y;
                var Z = loadResult.Vertices[face[0].VertexIndex - 1].Z;
                var A = new System.Numerics.Vector3(X, Y, Z);

                X = loadResult.Vertices[face[1].VertexIndex - 1].X;
                Y = loadResult.Vertices[face[1].VertexIndex - 1].Y;
                Z = loadResult.Vertices[face[1].VertexIndex - 1].Z;
                var B = new System.Numerics.Vector3(X, Y, Z);

                X = loadResult.Vertices[face[2].VertexIndex - 1].X;
                Y = loadResult.Vertices[face[2].VertexIndex - 1].Y;
                Z = loadResult.Vertices[face[2].VertexIndex - 1].Z;
                var C = new System.Numerics.Vector3(X, Y, Z);

                var Xn = loadResult.Normals[face[0].NormalIndex - 1].X;
                var Yn = loadResult.Normals[face[0].NormalIndex - 1].Y;
                var Zn = loadResult.Normals[face[0].NormalIndex - 1].Z;
                var An = new System.Numerics.Vector3(Xn, Yn, Zn);
                
                Xn = loadResult.Normals[face[1].NormalIndex - 1].X;
                Yn = loadResult.Normals[face[1].NormalIndex - 1].Y;
                Zn = loadResult.Normals[face[1].NormalIndex - 1].Z;
                var Bn = new System.Numerics.Vector3(Xn, Yn, Zn);

                Xn = loadResult.Normals[face[2].NormalIndex - 1].X;
                Yn = loadResult.Normals[face[2].NormalIndex - 1].Y;
                Zn = loadResult.Normals[face[2].NormalIndex - 1].Z;
                var Cn = new System.Numerics.Vector3(Xn, Yn, Zn);


                faces.Add(new Triangle(A, B, C, An, Bn, Cn));
            }

            ObjectModel.Faces = faces;
        }

        public static void GetTextures(LoadResult loadResult)
        {
            foreach (var texture in loadResult.Textures)
            {
                ObjectModel.Textures.Add(texture);
            }
        }
    }
}