using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ConverterBase;
using ConverterBase.GeomHelper;
using Priority_Queue;
using Raytracer.Optimisation;
using Raytracer.Scene.Interfaces;

namespace Raytracer.Tracing
{
    public class Raytracer: ITracer
    {   
        public List<List<Pixel>> Trace(ISceneCreator sceneCreator, ITreeProvider octree)
        {
            var camera = sceneCreator.ParamsProvider.Camera;
            var screenZ = sceneCreator.ParamsProvider.ScreenZ;
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var num = 0; 

            var image = new List<List<Pixel>>();
            for (var i = 0; i < sceneCreator.ParamsProvider.ImageHeight; i++)
            {
                image.Add(new List<Pixel>());
                var y = sceneCreator.SetXScreenCoordinate(i);
                for (var j = 0; j < sceneCreator.ParamsProvider.ImageWidth; j++)
                {
                    var x = sceneCreator.SetYScreenCoordinate(j);
                    
                    var pixelCenterPoint = new Vector3(x, y, screenZ);
                    var rayDirection = pixelCenterPoint - camera;
                    rayDirection = Vector3.Normalize(rayDirection);
                  
                    float t = 0;
                    var priorityQueue = new SimplePriorityQueue<INode, float>();
                    if (IsRayIntersectBox(octree.Root.MinBoundary, octree.Root.MaxBoundary, sceneCreator.ParamsProvider.Camera, rayDirection, ref t))
                    {
                        priorityQueue.Enqueue(octree.Root,t);
                    }

                    Triangle nearestTriangle = null;
                    var nearestTriangleIntersectionPoint = new Vector3();
                    var minDistance = float.MaxValue;
                    var barycentricIntersectionPoint = new Vector3();   
                    var intersectionPoint = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                    while (priorityQueue.Count != 0)
                    {
                        var flag = false;
                        var node = priorityQueue.Dequeue();
                        var intersectedTriangle =
                            FindIntersectionInBox(node.Faces, rayDirection, sceneCreator.ParamsProvider.Camera, ref num, ref intersectionPoint, ref barycentricIntersectionPoint);
                        var distanceBetweenCameraAndTriangle = Vector3.Distance(intersectionPoint, sceneCreator.ParamsProvider.Camera);
                        if (distanceBetweenCameraAndTriangle < minDistance)
                        {
                            minDistance = distanceBetweenCameraAndTriangle;
                            nearestTriangle = intersectedTriangle;
                            nearestTriangleIntersectionPoint = intersectionPoint;
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
                                .Where(child => IsRayIntersectBox(child.MinBoundary, child.MaxBoundary, sceneCreator.ParamsProvider.Camera, rayDirection, ref t)))
                            {
                                priorityQueue.Enqueue(child,t);
                            }
                        }
                    }
                    
                    if (nearestTriangle is null)
                    {
                        image[i].Add(new Pixel(232, 234, 246));
                        // var newRay = pixelCenterPoint - sceneCreator.ParamsProvider.LightPosition;
                        // t = 0;
                        // priorityQueue = new SimplePriorityQueue<INode, float>();
                        // if (IsRayIntersectBox(octree.Root.MinBoundary, octree.Root.MaxBoundary, pixelCenterPoint, newRay, ref t))
                        // {
                        //     priorityQueue.Enqueue(octree.Root,t);
                        // }
                        //
                        // var intersectionPoint = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                        //
                        // var intersectedTriangle = new Triangle();
                        //
                        // while (priorityQueue.Count != 0)
                        // {
                        //     var flag = false;
                        //     var node = priorityQueue.Dequeue();
                        //     intersectedTriangle =
                        //         FindIntersectionInBox(node.Faces, newRay, pixelCenterPoint, ref num, ref intersectionPoint, ref barycentricIntersectionPoint);
                        //     
                        //     
                        //     if (node.IsLeaf())
                        //     {
                        //         if (flag)
                        //             break;
                        //     }
                        //     else
                        //     {
                        //         foreach (var child in node.ChildNodes
                        //             .Where(child => IsRayIntersectBox(child.MinBoundary, child.MaxBoundary, pixelCenterPoint, newRay, ref t)))
                        //         {
                        //             priorityQueue.Enqueue(child,t);
                        //         }
                        //     }
                        // }
                        //
                        // if (intersectedTriangle != null && Vector3.Distance(pixelCenterPoint, sceneCreator.ParamsProvider.LightPosition) > Vector3.Distance(pixelCenterPoint, intersectionPoint))
                        // {
                        //     image[i].Add(new Pixel(0, 0, 0));
                        // }
                        // else
                        // {
                        //     image[i].Add(new Pixel(232, 234, 246));
                        // }
                        
                    }
                    else
                    {
                        // var normal = nearestTriangle.GetNormal();
                        var normal = nearestTriangle.GetBarycentricNormal(barycentricIntersectionPoint);
                        var lightRay = Vector3.Normalize(sceneCreator.ParamsProvider.LightPosition - nearestTriangleIntersectionPoint);
                        var dotProduct = Vector3.Dot(lightRay, normal);
                        var facingRatio = Math.Max(0, dotProduct);
                        
                        //var facingRatio = 0.18f / Math.PI * 30 * 0.5 * Math.Max(0f, dotProduct); 
                        image[i].Add(new Pixel((byte) (159 * facingRatio), (byte) (168 * facingRatio),
                            (byte) (218 * facingRatio)));
                    }
                }
            }

            watch.Stop();
            var time = watch.ElapsedMilliseconds / 1000;
            Console.WriteLine($"Exec time: {time} s");
            Console.WriteLine($"Num of intersection tests: {num}");
            
            return image;
        }
        
        private Triangle FindIntersectionInBox(List<Triangle> facesInBox, Vector3 rayDirection, Vector3 camera, ref int num, ref Vector3 intersectionPoint, ref Vector3 outBarycentricIntersectionPoint)
        {
            Triangle nearestTriangle = null;
            var minDistance = float.MaxValue;
            foreach (var triangle in facesInBox)
            {
                num++;
                if (triangle.IsIntersectTriangle(camera, rayDirection, ref intersectionPoint, ref outBarycentricIntersectionPoint))
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