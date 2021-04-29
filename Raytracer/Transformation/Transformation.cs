using System.Numerics;
using Raytracer.ObjectProvider;

namespace Raytracer.Transformation
{
    public static class Transformation
    {
        private static Matrix4x4 TransformationMatrix { get; set; } = Matrix4x4.Identity;
        
        private static readonly Matrix4x4 _translationM = new Matrix4x4 
        (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );
        private static readonly Matrix4x4 _scaleM = new Matrix4x4 
        (
            2f, 0, 0, 0,
            0, 2f, 0, 0,
            0, 0, 2f, 0,
            0, 0, 0, 1
        );
        private static readonly Matrix4x4 _rotationX = new Matrix4x4 
        (
            1, 0, 0, 0,
            0, 0, 1, 0,
            0, -1, 0, 0,
            0, 0, 0, 1
        );
        private static readonly Matrix4x4 _rotationY = new Matrix4x4 
        (
            -1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, -1, 0,
            0, 0, 0, 1
        );
        private static readonly Matrix4x4 _rotationZ = new Matrix4x4 
        (
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );
        
        public static void RotateX()
        {
            TransformationMatrix *= _rotationX;
        }

        public static void RotateY()
        {
            TransformationMatrix *= _rotationY;
        }

        public static void RotateZ()
        {
            TransformationMatrix *= _rotationZ;
        }

        public static void Translate()
        {
            TransformationMatrix *= _translationM;
        }

        public static void Scale()
        {
            TransformationMatrix *= _scaleM;
        }

        public static ObjectModel Transform(ObjectModel object3D)
        {
            for (var i = 0; i < object3D.Vertices.Count; i++)
            {
                var v3 = new Vector3(object3D.Vertices[i].X, object3D.Vertices[i].Y, object3D.Vertices[i].Z);
            
                var v4 = new Vector4(v3.X, v3.Y, v3.Z, 1);
                var res = MultiplyBy(v4);
            
                object3D.Vertices[i] = new Vector3(res.X, res.Y, res.Z);
            }

            return object3D;
        }
        
        private static Vector4 MultiplyBy(Vector4 v)
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