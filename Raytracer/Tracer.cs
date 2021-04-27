using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ConverterBase;
using ConverterBase.GeomHelper;

using Priority_Queue;
using Raytracer.Optimisation;
using Raytracer.Scene;


namespace Raytracer
{
    public class Tracer
    {
        private SceneCreator _sceneCreator;
        public static int ImageWidth;
        public static int ImageHeight;
        // public static int Fov = 80;
        public static Vector3 Camera => new Vector3(0, 0, 2);
        public static float ScreenZ => 1f;
        public static Vector3 LightPos => new Vector3(-2, 2, 0);
        
        // public static float ImageAspectRatio;

        public Tracer()
        {
            
        }
        public Tracer(SceneCreator scene)
        {
            _sceneCreator = scene;
        }
        
        public List<List<Pixel>> Trace(int width, int height, Octree octree)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ImageWidth = width;
            ImageHeight = height;    
            // ImageAspectRatio = ImageWidth / ImageHeight; 
            var num = 0; 

            var image = new List<List<Pixel>>();
            for (var i = 0; i < ImageHeight; i++)
            {
                image.Add(new List<Pixel>());
                var y = _sceneCreator.SetYCoordinate(i);
                for (var j = 0; j < ImageWidth; j++)
                {
                    var x = _sceneCreator.SetXCoordinate(j);
                    
                    var pixelCenterPoint = new Vector3(x, y, ScreenZ);
                    var rayDirection = pixelCenterPoint - Camera;
                    rayDirection = Vector3.Normalize(rayDirection);
                  
                    float t = 0;
                    var priorityQueue = new SimplePriorityQueue<OctreeNode, float>();
                    if (Extension.IsRayIntersectBox(octree.Root.Min, octree.Root.Max, Camera, rayDirection, ref t))
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
                            FindIntersectionInBox(node.Faces, rayDirection, ref num, ref intersectionPoint);
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
                            foreach (var child in node.ChildNodes
                                .Where(child => Extension.IsRayIntersectBox(child.Min, child.Max, Camera, rayDirection, ref t)))
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
                        
                        //var facingRatio = 0.18f / Math.PI * 30 * 0.5 * Math.Max(0f, dotProduct); 
                        
                        image[i].Add(new Pixel((byte) (255 * facingRatio), (byte) (69 * facingRatio),
                            (byte) (0 * facingRatio)));
                    }
                }
            }

            watch.Stop();
            var time = watch.ElapsedMilliseconds / 1000;
            Console.WriteLine($"Exec time: {time} s");
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
        
        private static Triangle FindIntersectionInBox(List<Triangle> facesInBox, Vector3 rayDirection, ref int num, ref Vector3 intersectionPoint)
        {
            Triangle nearestTriangle = null;
            var minDistance = float.MaxValue;
            foreach (var triangle in facesInBox)
            {
                num++;
                if (triangle.IsIntersectTriangle(Camera, rayDirection, ref intersectionPoint))
                {
                    var distanceBetweenCameraAndTriangle = Vector3.Distance(intersectionPoint, Camera);
                    
                    if (distanceBetweenCameraAndTriangle < minDistance)
                    {
                        nearestTriangle = triangle;
                        minDistance = distanceBetweenCameraAndTriangle;
                    }
                }
            }

            return nearestTriangle;
        }

        // Möller–Trumbore intersection algorithm 
        // rewrite from wiki
        // private static bool IsIntersectTriangle(Vector3 rayOrigin, Vector3 rayDirection, Triangle triangle,
        //     ref Vector3 outIntersectionPoint)
        // {
        //     const float EPSILON = (float) 0.0000001;
        //
        //     Vector3 edge1, edge2;
        //     Vector3 h, s, q;
        //     float a, f, u, v;
        //
        //     edge1 = triangle.B - triangle.A;
        //     edge2 = triangle.C - triangle.A;
        //
        //     h = Vector3.Cross(edge2, rayDirection);
        //     a = Vector3.Dot(h, edge1);
        //
        //     if (a > -EPSILON && a < EPSILON)
        //         return false;
        //
        //     f = (float) (1.0 / a);
        //     s = rayOrigin - triangle.A;
        //     u = f * Vector3.Dot(s, h);
        //
        //     if (u < 0.0 || u > 1.0)
        //         return false;
        //
        //     q = Vector3.Cross(edge1, s);
        //     v = f * Vector3.Dot(rayDirection, q);
        //     if (v < 0.0 || u + v > 1.0)
        //         return false;
        //
        //     float t = f * Vector3.Dot(edge2, q);
        //     if (t > EPSILON)
        //     {
        //         outIntersectionPoint = rayOrigin + rayDirection * t;
        //         return true;
        //     }
        //
        //     return false;
        // }

        // private static float XToScreenCoordinates(float x)
        // { 
        //     return (float) ((2 * ((x + 0.5) / ImageWidth) - 1) * Math.Tan(Fov / 2f * Math.PI / 180) * ImageAspectRatio);
        // }
        //
        // private static float YToScreenCoordinates(float y)
        // {
        //     return (float) ((1 - 2 * ((y + 0.5) / ImageHeight)) * Math.Tan(Fov / 2f * Math.PI / 180));
        // }
    }
}