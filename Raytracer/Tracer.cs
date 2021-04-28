using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using ConverterBase;
using ConverterBase.GeomHelper;

using Priority_Queue;
using Raytracer.Optimisation;
using Raytracer.Scene;


namespace Raytracer
{
    public class Tracer
    {
        private ISceneCreator _sceneCreator;

        public Tracer()
        {
            
        }
        public Tracer(ISceneCreator scene)
        {
            _sceneCreator = scene;
        }
        
        public List<List<Pixel>> Trace(float screenZ, Octree octree)
        {
            var camera = _sceneCreator.SetCamera();
            var light = _sceneCreator.SetLight();
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            _sceneCreator.CreateScreen();
            
            var num = 0; 

            var image = new List<List<Pixel>>();
            for (var i = 0; i < height; i++)
            {
                image.Add(new List<Pixel>());
                var y = _sceneCreator.SetXScreenCoordinate(i);
                for (var j = 0; j < width; j++)
                {
                    var x = _sceneCreator.SetYScreenCoordinate(j);
                    
                    var pixelCenterPoint = new Vector3(x, y, screenZ);
                    var rayDirection = pixelCenterPoint - camera;
                    rayDirection = Vector3.Normalize(rayDirection);
                  
                    float t = 0;
                    var priorityQueue = new SimplePriorityQueue<OctreeNode, float>();
                    if (IsRayIntersectBox(octree.Root.Min, octree.Root.Max, camera, rayDirection, ref t))
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
                            FindIntersectionInBox(node.Faces, rayDirection, camera, ref num, ref intersectionPoint);
                        var distanceBetweenCameraAndTriangle = Vector3.Distance(intersectionPoint, camera);
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
                                .Where(child => IsRayIntersectBox(child.Min, child.Max, camera, rayDirection, ref t)))
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
                        var lightRay = Vector3.Normalize(light - nearestTriangleIntersectionPoint);
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
        
        private Triangle FindIntersectionInBox(List<Triangle> facesInBox, Vector3 rayDirection, Vector3 camera, ref int num, ref Vector3 intersectionPoint)
        {
            Triangle nearestTriangle = null;
            var minDistance = float.MaxValue;
            foreach (var triangle in facesInBox)
            {
                num++;
                if (triangle.IsIntersectTriangle(camera, rayDirection, ref intersectionPoint))
                {
                    var distanceBetweenCameraAndTriangle = Vector3.Distance(intersectionPoint, camera);
                    
                    if (distanceBetweenCameraAndTriangle < minDistance)
                    {
                        nearestTriangle = triangle;
                        minDistance = distanceBetweenCameraAndTriangle;
                    }
                }
            }

            return nearestTriangle;
        }
        
        private bool IsRayIntersectBox(Vector3 pMin, Vector3 pMax, Vector3 origin, Vector3 direction, ref float t)
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
    }
}