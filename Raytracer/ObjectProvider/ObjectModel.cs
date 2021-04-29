using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Data.VertexData;

namespace Raytracer.ObjectProvider
{
    public class ObjectModel
    {
        public List<Vector3> Vertices { get; }
        public List<Vector3> VerticesNormals { get; }
        public List<Triangle> Faces { get; set; }
        public List<Texture> Textures { get; }

        public ObjectModel()
        {
            Vertices = new List<Vector3>();
            VerticesNormals = new List<Vector3>();
            Faces = new List<Triangle>();
            Textures = new List<Texture>();
        }
        
    }
}