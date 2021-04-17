using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ConverterBase;
using ConverterBase.GeomHelper;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Loaders;

// using OBJ3DWavefrontLoader;

namespace Raytracer
{
    public static class Tracer
    {
        public static int ImageWidth;
        public static int ImageHeight;
        public static int Fov = 80;
        public static Vector3 Camera => new Vector3(0, 0, 2);
        public static float ScreenZ => 1f;
        public static Vector3 LightPos => new Vector3(0, 4, 2);
        
        public static float ImageAspectRatio;
        
        public static List<List<Pixel>> Trace(int width, int height, LoadResult object3D)
        {
            ImageWidth = width;
            ImageHeight = height;    
            ImageAspectRatio = ImageWidth / ImageHeight;

            var image = new List<List<Pixel>>();
            for (var i = 0; i < ImageHeight; i++)
            {
                image.Add(new List<Pixel>());
                var y = YToScreenCoordinates(i);
                for (var j = 0; j < ImageWidth; j++)
                {
                    var x = XToScreenCoordinates(j);
                    
                    var pixelCenterPoint = new Vector3(x, y, ScreenZ);
                    var rayDirection = pixelCenterPoint - Camera;
                    Vector3.Normalize(rayDirection);
                    
                    var minDistance = float.MaxValue;
                    Triangle nearestTriangle = null;
                    var intersectionPoint = new Vector3();

                    // Triangle triangle = new Triangle(new Vector3(0, 0, -3), new Vector3(-2, 1, -3), new Vector3(1, 1, -3));
                    foreach (var face in object3D.Groups[0].Faces)
                    {
                        var X = object3D.Vertices[face[0].VertexIndex - 1].X;
                        var Y = object3D.Vertices[face[0].VertexIndex - 1].Y;
                        var Z = object3D.Vertices[face[0].VertexIndex - 1].Z;
                        var A = new Vector3(X,Y,Z);
                        
                        X = object3D.Vertices[face[1].VertexIndex - 1].X;
                        Y = object3D.Vertices[face[1].VertexIndex - 1].Y;
                        Z = object3D.Vertices[face[1].VertexIndex - 1].Z;
                        var B = new Vector3(X,Y,Z);
                        
                        X = object3D.Vertices[face[2].VertexIndex - 1].X;
                        Y = object3D.Vertices[face[2].VertexIndex - 1].Y;
                        Z = object3D.Vertices[face[2].VertexIndex - 1].Z;
                        var C = new Vector3(X,Y,Z);
                        
                        var triangle = new Triangle(A, B, C);

                        if (IsIntersectTriangle(Camera, rayDirection, triangle, ref intersectionPoint))
                        {
                            var distanceBetweenCameraAndTriangle = Vector3.Distance(intersectionPoint, Camera);

                            if (distanceBetweenCameraAndTriangle < minDistance)
                            {
                                nearestTriangle = triangle;
                                minDistance = distanceBetweenCameraAndTriangle;
                            }
                        }
                    }

                    if (nearestTriangle is null)
                    {
                        image[i].Add(new Pixel(50, 50, 50));
                    }
                    else
                    {
                        var normal = nearestTriangle.GetNormal();
                        var lightRay = Vector3.Normalize(LightPos - intersectionPoint);
                        var dotProduct = Vector3.Dot(lightRay, normal);
                        var facingRatio = Math.Max(0, dotProduct);

                        image[i].Add(new Pixel((byte)(255 * facingRatio), (byte)(69 * facingRatio), (byte)(0 * facingRatio)));
                    }
                }
            }

            return image;
        }

        // find intersection with sphere
        private static bool IsSphereIntersect(Vector3 origin, Vector3 direction)
        {
            // sphere equation
            Vector3 center = new Vector3(0, 0, 1);
            var r = .5f;
            Vector3 k = origin - center;

            var a = Vector3.Dot(direction, direction);
            var b = 2 * Vector3.Dot(direction, k);
            var c = Vector3.Dot(k, k) - r * r;

            var D = b * b - 4 * a * c;
            
            return D >=0 ;
        }

        
        // Möller–Trumbore intersection algorithm 
        // rewrite from wiki
        private static bool IsIntersectTriangle(Vector3 rayOrigin, Vector3 rayDirection, Triangle triangle,
            ref Vector3 outIntersectionPoint)
        {
            const float EPSILON = (float) 0.0000001;

            Vector3 edge1, edge2;
            Vector3 h, s, q;
            float a, f, u, v;

            edge1 = triangle.B - triangle.A;
            edge2 = triangle.C - triangle.A;

            h = Vector3.Cross(edge2, rayDirection);
            a = Vector3.Dot(h, edge1);

            if (a > -EPSILON && a < EPSILON)
                return false;

            f = (float) (1.0 / a);
            s = rayOrigin - triangle.A;
            u = f * Vector3.Dot(s, h);

            if (u < 0.0 || u > 1.0)
                return false;

            q = Vector3.Cross(edge1, s);
            v = f * Vector3.Dot(rayDirection, q);
            if (v < 0.0 || u + v > 1.0)
                return false;

            float t = f * Vector3.Dot(edge2, q);
            if (t > EPSILON)
            {
                outIntersectionPoint = rayOrigin + rayDirection * t;
                return true;
            }

            return false;
        }

        private static float XToScreenCoordinates(float x)
        { 
            return (float) ((2 * ((x + 0.5) / ImageWidth) - 1) * Math.Tan(Fov / 2f * Math.PI / 180) * ImageAspectRatio);
        }

        private static float YToScreenCoordinates(float y)
        {
            return (float) ((1 - 2 * ((y + 0.5) / ImageHeight)) * Math.Tan(Fov / 2f * Math.PI / 180));
        }
    }
}