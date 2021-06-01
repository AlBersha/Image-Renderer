using System.Numerics;

namespace ConverterBase.GeomHelper
{
    public class Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        public Vector3 An;
        public Vector3 Bn;
        public Vector3 Cn;

        public Triangle()
        {
            
        }
        
        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }
        
        public Triangle(Vector3 a, Vector3 b, Vector3 c, Vector3 an, Vector3 bn, Vector3 cn)
        {
            A = a;
            B = b;
            C = c;

            An = Vector3.Normalize(an);
            Bn = Vector3.Normalize(bn);
            Cn = Vector3.Normalize(cn);
        }

        public Vector3 GetNormal()
        {
            return Vector3.Normalize(Vector3.Cross(B - A, C - A));
        }

        public Vector3 GetBarycentricNormal(Vector3 point)
        {
            if (An == Vector3.Zero || Bn == Vector3.Zero || Cn == Vector3.Zero )
            {
                return GetNormal();
            }
            return Vector3.Normalize(point.X * An + point.Y * Bn + point.Z * Cn);
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
            ref Vector3 outIntersectionPoint, ref Vector3 outBarycentricIntersectionPoint)
        {
            const float EPSILON = (float) 0.0000001;

            var edge1 = B - A;
            var edge2 = C - A;

            var h = Vector3.Cross(rayDirection, edge2);
            var a = Vector3.Dot(h, edge1);

            if (a > -EPSILON && a < EPSILON)
                return false;

            var f = (float) (1.0 / a);
            var s = rayOrigin - A;
            var u = f * Vector3.Dot(s, h);

            if (u < 0.0 || u > 1.0)
                return false;

            var q = Vector3.Cross(s, edge1);
            var v = f * Vector3.Dot(rayDirection, q);
            if (v < 0.0 || u + v > 1.0)
                return false;

            var t = f * Vector3.Dot(edge2, q);
            if (t > EPSILON)
            {
                outIntersectionPoint = rayOrigin + rayDirection * t;

                outBarycentricIntersectionPoint.X = 1 - u - v;
                outBarycentricIntersectionPoint.Y = u;
                outBarycentricIntersectionPoint.Z = v;
                
                return true;
            }

            return false;
        }
    }
}