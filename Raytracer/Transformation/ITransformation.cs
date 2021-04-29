// using System;
// using System.Numerics;
// using ObjLoader.Loader.Loaders;
// using Raytracer.ObjectProvider;
//
// namespace Raytracer.Transformation
// {
//     public interface ITransformation
//     {
//         public Matrix4x4 TransformationMatrix { get; set; }
//         public void RotateX(Single angle = 90);
//         public void RotateY(Single angle = 90);
//         public void RotateZ(Single angle = 90);
//         public void Translate(Vector3 translationVector);
//         public void Scale(Single scaleTimes = 2f);
//         public void Transform(ref ObjectModel object3D);
//     }
// }