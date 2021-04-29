using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer.Optimisation
{
    public interface INode
    {
        public Vector3 MinBoundary { get; }
        public Vector3 MaxBoundary { get; }
        public List<Triangle> Faces { get; set; }
        public List<INode> ChildNodes { get; }
        
        
        public void DivideIntoBoxes();
        public bool IsLeaf();
        public List<Triangle> FindTrianglesInBox(Vector3 pMin, Vector3 pMax);
    }
}