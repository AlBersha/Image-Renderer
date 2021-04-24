using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using ConverterBase;
using ConverterBase.GeomHelper;
using Microsoft.VisualBasic;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using Priority_Queue;


namespace Raytracer
{
    public static class Tracer
    {
        public static int ImageWidth;
        public static int ImageHeight;
        public static int Fov = 80;
        public static Vector3 Camera => new Vector3(0, 0, 2);
        public static float ScreenZ => 1f;
        public static Vector3 LightPos => new Vector3(0, 0, 2);
        
        public static float ImageAspectRatio;
        
        public static List<List<Pixel>> Trace(int width, int height, LoadResult object3D, Vector3 min, Vector3 max)
        {
            var wath = System.Diagnostics.Stopwatch.StartNew();
            ImageWidth = width;
            ImageHeight = height;    
            ImageAspectRatio = ImageWidth / ImageHeight;
            var g = 0;
            var num = 0; 
            var octree = new Octree(min, max, object3D.Groups[0].Faces, object3D.Vertices);

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
                    rayDirection = Vector3.Normalize(rayDirection);
                  
                    float t = 0;
                    var priorityQueue = new SimplePriorityQueue<OctreeNode, float>();
                    if (Extension.IsRayIntersectBox(octree.Root.pMin, octree.Root.pMax, Camera, rayDirection, ref t))
                    {
                        priorityQueue.Enqueue(octree.Root,t);
                    }

                    Triangle nearestTriangle = null;
                    var nearestTriangleIntersectionPoint = new Vector3();
                    var minDistance = float.MaxValue;
                    while (priorityQueue.Count != 0)
                    {
                        var flag = false;
                        var node = priorityQueue.Dequeue();
                        var intersectionPoint = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                        var intersectedTriangle =
                            FindIntersectionInBox(node.Faces, object3D.Vertices, rayDirection, ref num, ref intersectionPoint);
                        var distanceBetweenCameraAndTriangle = Vector3.Distance(intersectionPoint, Camera);
                        if (distanceBetweenCameraAndTriangle < minDistance)
                        {
                            minDistance = distanceBetweenCameraAndTriangle;
                            nearestTriangle = intersectedTriangle;
                            flag = true;
                        }
                        
                        if (node.IsLeaf())
                        {
                            if (flag)
                                break;
                        }
                        else
                        {
                            foreach (var child in node.childNodes
                                .Where(child => Extension.IsRayIntersectBox(child.pMin, child.pMax, Camera, rayDirection, ref t)))
                            {
                                priorityQueue.Enqueue(child,t);
                            }
                        }
                    }
                    
                    if (nearestTriangle is null)
                    {
                        image[i].Add(new Pixel(20, 20, 20));
                    }
                    else
                    {
                        var normal = nearestTriangle.GetNormal();
                        var lightRay = Vector3.Normalize(LightPos - nearestTriangleIntersectionPoint);
                        var dotProduct = Vector3.Dot(lightRay, normal);
                        var facingRatio = Math.Max(0, dotProduct);
                        //
                        // var X = object3D.Vertices[g[0].NormalIndex - 1].X;
                        // var Y = object3D.Vertices[g[0].NormalIndex - 1].Y;
                        // var Z = object3D.Vertices[g[0].NormalIndex - 1].Z;
                        // var n0 = Vector3.Normalize(new Vector3(X,Y,Z));
                        //
                        // X = object3D.Vertices[g[1].NormalIndex - 1].X;
                        // Y = object3D.Vertices[g[1].NormalIndex - 1].Y;
                        // Z = object3D.Vertices[g[1].NormalIndex - 1].Z;
                        // var n1 = Vector3.Normalize(new Vector3(X,Y,Z));
                        //
                        // X = object3D.Vertices[g[2].NormalIndex - 1].X;
                        // Y = object3D.Vertices[g[2].NormalIndex - 1].Y;
                        // Z = object3D.Vertices[g[2].NormalIndex - 1].Z;
                        // var n2 = Vector3.Normalize(new Vector3(X,Y,Z));
                        //
                        // var smoothNormal = Vector3.Normalize((1 - uv.X - uv.Y) * n0 + uv.X * n1 + uv.Y * n2);
                        //
                        // var lightRay = Vector3.Normalize(LightPos - intersectionPoint);
                        // var dotProduct = Vector3.Dot(lightRay, smoothNormal);
                        // var facingRatio = Math.Max(0, dotProduct);

                        image[i].Add(new Pixel((byte) (255 * facingRatio), (byte) (69 * facingRatio),
                            (byte) (0 * facingRatio)));
                    }
                    
                    //--------------------------------------------------------
                }
            }

            wath.Stop();
            var time = wath.ElapsedMilliseconds / 1000;
            Console.WriteLine($"Exec time: {time} s");
            Console.WriteLine($"Num of triangle: {object3D.Groups[0].Faces.Count}");
            Console.WriteLine($"Num of intersection tests: {num}");
            
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

            return D >= 0;
        }
        
        private static Triangle FindIntersectionInBox(List<Face> facesInBox, IList<Vertex> vertices,
            Vector3 rayDirection, ref int num, ref Vector3 intersectionPoint)
        {
            Triangle nearestTriangle = null;
            var minDistance = float.MaxValue;
            Vector2 uv = new Vector2();
            foreach (var face in facesInBox)
            {
                num++;

                var X = vertices[face[0].VertexIndex - 1].X;
                var Y = vertices[face[0].VertexIndex - 1].Y;
                var Z = vertices[face[0].VertexIndex - 1].Z;
                var A = new Vector3(X, Y, Z);

                X = vertices[face[1].VertexIndex - 1].X;
                Y = vertices[face[1].VertexIndex - 1].Y;
                Z = vertices[face[1].VertexIndex - 1].Z;
                var B = new Vector3(X, Y, Z);

                X = vertices[face[2].VertexIndex - 1].X;
                Y = vertices[face[2].VertexIndex - 1].Y;
                Z = vertices[face[2].VertexIndex - 1].Z;
                var C = new Vector3(X, Y, Z);

                var triangle = new Triangle(A, B, C);

                float u = 0, v = 0;
                if (IsIntersectTriangle(Camera, rayDirection, triangle, ref intersectionPoint, ref u,
                    ref v))
                {
                    var distanceBetweenCameraAndTriangle = Vector3.Distance(intersectionPoint, Camera);
                    
                    if (distanceBetweenCameraAndTriangle < minDistance)
                    {
                        uv.X = u;
                        uv.Y = v;
                        nearestTriangle = triangle;
                        minDistance = distanceBetweenCameraAndTriangle;
                    }
                }
            }

            return nearestTriangle;
        }

        // Möller–Trumbore intersection algorithm 
        // rewrite from wiki
        private static bool IsIntersectTriangle(Vector3 rayOrigin, Vector3 rayDirection, Triangle triangle,
            ref Vector3 outIntersectionPoint, ref float u, ref float v)
        {
            const float EPSILON = (float) 0.0000001;

            Vector3 edge1, edge2;
            Vector3 h, s, q;
            float a, f;

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