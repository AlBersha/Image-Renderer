using System.Collections.Generic;
using System.Numerics;

namespace ConverterBase.GeomHelper
{
    public class Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Vector3 GetNormal()
        {
            return Vector3.Normalize(Vector3.Cross(B - A, C - A));
        }

        public Vector3 GetCentroid()
        {
            return new Vector3((A.X + B.X + C.X) / 3, (A.Y + B.Y + C.Y) / 3, (A.Z + B.Z + C.Z) / 3);
        }
        
        public bool IsTriangleInBox(Vector3 pMin, Vector3 pMax)
        {
            return IsDotInBox(pMin, pMax, A) &&
                   IsDotInBox(pMin, pMax, B) &&
                   IsDotInBox(pMin, pMax, C);
        }
        
        private bool IsDotInBox(Vector3 pMin, Vector3 pMax, Vector3 dot)
        {
            return pMin.X <= dot.X && dot.X <= pMax.X && 
                   pMin.Y <= dot.Y && dot.Y <= pMax.Y &&
                   pMin.Z <= dot.Z && dot.Z <= pMax.Z;
        }
        
        // Möller–Trumbore intersection algorithm 
        // rewrite from wiki
        public bool IsIntersectTriangle(Vector3 rayOrigin, Vector3 rayDirection,
            ref Vector3 outIntersectionPoint)
        {
            const float EPSILON = (float) 0.0000001;

            Vector3 edge1, edge2;
            Vector3 h, s, q;
            float a, f, u, v;

            edge1 = B - A;
            edge2 = C - A;

            h = Vector3.Cross(edge2, rayDirection);
            a = Vector3.Dot(h, edge1);

            if (a > -EPSILON && a < EPSILON)
                return false;

            f = (float) (1.0 / a);
            s = rayOrigin - A;
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
    }
}