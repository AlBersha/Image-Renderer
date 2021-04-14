using System;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer
{
    public static class Tracer
    {
        public static int ImageWidth = 30;
        public static int ImageHeight = 30;
        public static int Fov = 90;
        public static Vector3 Camera => new Vector3(0, 0, 0);
        
        public static float ImageAspectRatio => ImageWidth / ImageHeight;
        
        public static Point3[,] Trace()
        {
            Point3[,] resMatrix = new Point3[ImageWidth, ImageHeight];
            for (int j = 0; j < ImageHeight; j++)
            {
                float y = YToScreenCoordinates(j);
                for (int i = 0; i < ImageWidth; i++)
                {
                    float x = XToScreenCoordinates(i);
                    
                    Vector3 rayDirection = new Vector3(x, y, -1) - Camera;
                    Vector3.Normalize(rayDirection);

                    Triangle triangle = new Triangle(new Vector3(0, 0, -3), new Vector3(-2, 1, -3), new Vector3(1, 1, -3));
                    Vector3 intersectionPoint = new Vector3();

                    if (IsIntersectTriangle(Camera, rayDirection, triangle, ref intersectionPoint))
                    {
                        // resMatrix[i, j] = true;
                        resMatrix[i, j] = new Point3(255, 0, 0);
                    }
                    else
                    {
                        // resMatrix[i, j] = false;
                        resMatrix[i, j] = new Point3(255, 255, 255);
                    }
                }
            }

            return resMatrix;
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
        private static bool IsIntersectTriangle(Vector3 rayOrigin, Vector3 rayDirection, Triangle triangle, ref Vector3 outIntersectionPoint)
        {
            const float EPSILON = (float) 0.0000001;
            
            Vector3 edge1, edge2;
            Vector3 h, s, q;
            float a,f,u,v;
            
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
            return (float) ((2 * ((x + 0.5) / ImageWidth) - 1) * Math.Tan(Fov / 2 * Math.PI / 180) * ImageAspectRatio);
        }

        private static float YToScreenCoordinates(float y)
        {
            return (float) (1 - 2 * ((y + 0.5) / ImageHeight) * Math.Tan(Fov / 2 * Math.PI / 180));
        }
        
        
        
        
        
    }
}