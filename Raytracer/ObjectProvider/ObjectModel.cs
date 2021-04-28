using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Data.VertexData;

namespace Raytracer.ObjectProvider
{
    public class ObjectModel
    {
        public List<Vector3> Vertices { get; set; }
        public List<Vector3> VerticesNormals { get; set; }
        public List<Triangle> Faces { get; set; }
        public List<Texture> Textures { get; set; }
        
    }
}