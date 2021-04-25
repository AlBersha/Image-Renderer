using System;
using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;

namespace Raytracer
{
    public static class Extension
    {
        public static bool IsTriangleInBox(Vector3 pMin, Vector3 pMax, Triangle triangle)
        {
            return IsDotInBox(pMin, pMax, triangle.A) &&
                   IsDotInBox(pMin, pMax, triangle.B) &&
                   IsDotInBox(pMin, pMax, triangle.C);
        }

        public static bool IsDotInBox(Vector3 pMin, Vector3 pMax, Vector3 dot)
        {
            return pMin.X <= dot.X && dot.X <= pMax.X && 
                   pMin.Y <= dot.Y && dot.Y <= pMax.Y &&
                   pMin.Z <= dot.Z && dot.Z <= pMax.Z;
        }
        public static List<Triangle> FindTrianglesInBox(Vector3 pMin, Vector3 pMax, ref List<Triangle> faces)
        {
            var faceInBox = new List<Triangle>();
            var faceNotInBox = new List<Triangle>();
            foreach (var face in faces)
            {
                if (IsTriangleInBox(pMin, pMax, face))
                {
                    faceInBox.Add(face);
                }
                else
                {
                    faceNotInBox.Add(face);
                }
            }

            faces.Clear();
            faces.AddRange(faceNotInBox);
            return faceInBox;
        }
        public static bool IsRayIntersectBox(Vector3 pMin, Vector3 pMax, Vector3 origin, Vector3 direction, ref float t)
        {
            var t1 = (pMin.X - origin.X) * (1f / direction.X);
            var t2 = (pMax.X - origin.X) * (1f / direction.X);
            var t3 = (pMin.Y - origin.Y) * (1f / direction.Y);
            var t4 = (pMax.Y - origin.Y) * (1f / direction.Y);
            var t5 = (pMin.Z - origin.Z) * (1f / direction.Z);
            var t6 = (pMax.Z - origin.Z) * (1f / direction.Z);

            var tMin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
            var tMax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));

            // if tmax < 0, ray (line) is intersecting AABB, but the whole AABB is behind us
            if (tMax < 0)
            {
                t = tMax;
                return false;
            }

            // if tmin > tmax, ray doesn't intersect AABB
            if (tMin > tMax)
            {
                t = tMax;
                return false;
            }
            
            // if tmin < 0 then the ray origin is inside of the AABB and tmin is behind the start of the ray so tmax is the first intersection
            t = tMin < 0 ? tMax : tMin;
            return true;
        }
        public static List<Triangle> GetTrianglesList(LoadResult object3D)
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

            return faces;
        }
        public static void Transform(ref LoadResult object3D, Matrix4x4 rotationZ, Matrix4x4 rotationY, Matrix4x4 rotationX, Matrix4x4 scaleM, Matrix4x4 translationM)
        {
            var transformM = rotationZ * rotationY * rotationX * scaleM * translationM;
            
            for (var i = 0; i < object3D.Vertices.Count; i++)
            {
                var v3 = new Vector3(object3D.Vertices[i].X, object3D.Vertices[i].Y, object3D.Vertices[i].Z);
            
                var v4 = new Vector4(v3.X, v3.Y, v3.Z, 1);
                var res = transformM.MultiplyBy(v4);
            
                object3D.Vertices[i] = new Vertex(res.X, res.Y, res.Z);
            }
        }

        public static void BoundingBoxCoordinates(LoadResult object3D, ref Vector3 pMin, ref Vector3 pMax)
        {
            var xMin = float.MaxValue;
            var yMin = float.MaxValue;
            var zMin = float.MaxValue;
            var xMax = float.MinValue;
            var yMax = float.MinValue;
            var zMax = float.MinValue;
            
            foreach (var t in object3D.Vertices)
            {
                xMin = Math.Min(xMin, t.X);
                yMin = Math.Min(yMin, t.Y);
                zMin = Math.Min(zMin, t.Z);
                xMax = Math.Max(xMax, t.X);
                yMax = Math.Max(yMax, t.Y);
                zMax = Math.Max(zMax, t.Z);
            }

            pMin = new Vector3(xMin, yMin, zMin);
            pMax = new Vector3(xMax, yMax, zMax);
        }
    }
}