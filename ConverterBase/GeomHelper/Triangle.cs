using System.Numerics;

namespace ConverterBase.GeomHelper
{
    public class Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
        public Vector3 Center;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
            Center = new Vector3((A.X + B.X + C.X) / 3, (A.Y + B.Y + C.Y) / 3, (A.Z + B.Z + C.Z) / 3);
        }

        public Vector3 GetNormal()
        {
            return Vector3.Normalize(Vector3.Cross(B - A, C - A));
        }

        public Vector3 GetCentroid()
        {
            return new Vector3((A.X + B.X + C.X) / 3, (A.Y + B.Y + C.Y) / 3, (A.Z + B.Z + C.Z) / 3);
        }
    }
}