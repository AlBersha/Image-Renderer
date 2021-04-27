using System;
using System.Numerics;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;

namespace Raytracer.Transformation
{
    public class Transformation: ITransformation
    {
        public Matrix4x4 TransformationMatrix { get; set; }
        
        public void RotateX(float angle, Vector3 center)
        {
            throw new NotImplementedException();
        }

        public void RotateY(float angle, Vector3 center)
        {
            throw new NotImplementedException();
        }

        public void RotateZ(float angle, Vector3 center)
        {
            throw new NotImplementedException();
        }

        public void Translate(Vector3 translationVector)
        {
            throw new NotImplementedException();
        }

        public void Scale(float scaleTimes)
        {
            throw new NotImplementedException();
        }

        public void Transform(ref LoadResult object3D)
        {
            for (var i = 0; i < object3D.Vertices.Count; i++)
            {
                var v3 = new Vector3(object3D.Vertices[i].X, object3D.Vertices[i].Y, object3D.Vertices[i].Z);
            
                var v4 = new Vector4(v3.X, v3.Y, v3.Z, 1);
                var res = TransformationMatrix.MultiplyBy(v4);
            
                object3D.Vertices[i] = new Vertex(res.X, res.Y, res.Z);
            }
        }
    }
}