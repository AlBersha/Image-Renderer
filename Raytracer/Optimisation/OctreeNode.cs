using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer.Optimisation
{
    public class OctreeNode: INode
    {
        public List<INode> ChildNodes { get; set; }
        public List<Triangle> Faces { get; set; }
        public Vector3 MinBoundary { get; }
        public Vector3 MaxBoundary { get; }

        public OctreeNode(Vector3 pMin, Vector3 pMax, List<Triangle> faces, OctreeNode parent)
        {
            MinBoundary = pMin;
            MaxBoundary = pMax;
            Faces = new List<Triangle>(faces);

            ChildNodes = null;
        }

        public void DivideIntoBoxes()
        {
            if (Faces.Count < 10)
                return;
            
            ChildNodes = new List<INode>(8);
            var centroid = GetCentroid();

            //bottomFrontLeft
            var boxMin = new Vector3(MinBoundary.X, MinBoundary.Y, centroid.Z);
            var boxMax = new Vector3(centroid.X, centroid.Y, MaxBoundary.Z);
            var faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces, this));
            ChildNodes[0].DivideIntoBoxes();
            //bottomFrontRight
            boxMin = new Vector3(centroid.X, MinBoundary.Y, centroid.Z);
            boxMax = new Vector3(MaxBoundary.X, centroid.Y, MaxBoundary.Z);
            faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[1].DivideIntoBoxes();
            //topFrontLeft
            boxMin = new Vector3(MinBoundary.X, centroid.Y, centroid.Z);
            boxMax = new Vector3(centroid.X, MaxBoundary.Y, MaxBoundary.Z);
            faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[2].DivideIntoBoxes();
            //topFrontRight
            boxMin = centroid;
            boxMax = MaxBoundary;
            faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[3].DivideIntoBoxes();
            //bottomBackLeft
            boxMin = MinBoundary;
            boxMax = centroid;
            faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[4].DivideIntoBoxes();
            //bottomBackRight
            boxMin = new Vector3(centroid.X, MinBoundary.Y, MinBoundary.Z);
            boxMax = new Vector3(MaxBoundary.X, centroid.Y, centroid.Z);
            faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[5].DivideIntoBoxes();
            //topBackLeft
            boxMin = new Vector3(MinBoundary.X, centroid.Y, MinBoundary.Z);
            boxMax = new Vector3(centroid.X, MaxBoundary.Y, centroid.Z);
            faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[6].DivideIntoBoxes();
            //topBackRight
            boxMin = new Vector3(centroid.X, centroid.Y, MinBoundary.Z);
            boxMax = new Vector3(MaxBoundary.X, MaxBoundary.Y, centroid.Z);
            faces = FindTrianglesInBox(boxMin, boxMax);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[7].DivideIntoBoxes();
        }

        private Vector3 GetCentroid()
        {
            return new Vector3((MaxBoundary.X + MinBoundary.X) / 2, (MaxBoundary.Y + MinBoundary.Y) / 2, (MaxBoundary.Z + MinBoundary.Z) / 2);
        }

        public bool IsLeaf()
        {
            return ChildNodes == null;
        }

        public List<Triangle> FindTrianglesInBox(Vector3 pMin, Vector3 pMax)
        {
            var faceInBox = new List<Triangle>();
            var faceNotInBox = new List<Triangle>();
            foreach (var face in Faces)
            {
                if (face.IsTriangleInBox(pMin, pMax))
                {
                    faceInBox.Add(face);
                }
                else
                {
                    faceNotInBox.Add(face);
                }
            }

            Faces.Clear();
            Faces.AddRange(faceNotInBox);
            return faceInBox;
        }

    }
}