using System;
using System.Numerics;
using ObjLoader.Loader.Loaders;

namespace Raytracer.Transformation
{
    public interface ITransformation
    {
        public Matrix4x4 TransformationMatrix { get; set; }
        public void RotateX(Single angle,  Vector3 center);
        public void RotateY(Single angle,  Vector3 center);
        public void RotateZ(Single angle,  Vector3 center);
        public void Translate(Vector3 translationVector);
        public void Scale(Single scaleTimes);
        public void Transform(ref LoadResult object3D);
    }
}