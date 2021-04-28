using System.Numerics;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;

namespace Raytracer.Transformation
{
    public class Transformation: ITransformation
    {
        public Matrix4x4 TransformationMatrix { get; set; }
        
        public void RotateX(float angle = 90, Vector3 center = default)
        {
            TransformationMatrix *= Matrix4x4.CreateRotationX(angle, center);
        }

        public void RotateY(float angle = 90, Vector3 center = default)
        {
            TransformationMatrix *= Matrix4x4.CreateRotationY(angle, center);
        }

        public void RotateZ(float angle = 90, Vector3 center = default)
        {
            TransformationMatrix *= Matrix4x4.CreateRotationZ(angle, center);
        }

        public void Translate(Vector3 translationVector)
        {
            TransformationMatrix *= Matrix4x4.CreateTranslation(translationVector);
        }

        public void Scale(float scale)
        {
            TransformationMatrix *= Matrix4x4.CreateScale(scale);
        }

        //todo change object structure 
        public void Transform(ref LoadResult object3D)
        {
            for (var i = 0; i < object3D.Vertices.Count; i++)
            {
                var v3 = new Vector3(object3D.Vertices[i].X, object3D.Vertices[i].Y, object3D.Vertices[i].Z);
            
                var v4 = new Vector4(v3.X, v3.Y, v3.Z, 1);
                var res = MultiplyBy(v4);
            
                object3D.Vertices[i] = new Vertex(res.X, res.Y, res.Z);
            }
        }
        
        private Vector4 MultiplyBy(Vector4 v)
        {
            return new Vector4(
                TransformationMatrix.M11 * v.X + TransformationMatrix.M12 * v.Y + TransformationMatrix.M13 * v.Z + TransformationMatrix.M14 * v.W,
                TransformationMatrix.M21 * v.X + TransformationMatrix.M22 * v.Y + TransformationMatrix.M23 * v.Z + TransformationMatrix.M24 * v.W,
                TransformationMatrix.M31 * v.X + TransformationMatrix.M32 * v.Y + TransformationMatrix.M33 * v.Z + TransformationMatrix.M34 * v.W,
                TransformationMatrix.M41 * v.X + TransformationMatrix.M42 * v.Y + TransformationMatrix.M43 * v.Z + TransformationMatrix.M44 * v.W
            );
        }
    }
}