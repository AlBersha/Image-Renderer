using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer.Optimisation
{
    public interface INode
    {
        public void DivideIntoBoxes();
        public bool IsLeaf();
        public List<Triangle> FindTrianglesInBox(Vector3 pMin, Vector3 pMax, ref List<Triangle> faces);
    }
}