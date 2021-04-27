using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer.Optimisation
{
    public class OctreeNode: INode
    {
        public List<OctreeNode> ChildNodes;
        //public OctreeNode Parent;
        public List<Triangle> Faces;

        public Vector3 Min;
        public Vector3 Max;
        public OctreeNode(Vector3 pMin, Vector3 pMax, List<Triangle> faces, OctreeNode parent)
        {
            Min = pMin;
            Max = pMax;
            Faces = new List<Triangle>(faces);

           // Parent = parent;
            ChildNodes = null;
        }

        public void DivideIntoBoxes()
        {
            if (Faces.Count < 10)
                return;
            
            ChildNodes = new List<OctreeNode>(8);
            var centroid = GetCentroid();

            //bottomFrontLeft
            var boxMin = new Vector3(Min.X, Min.Y, centroid.Z);
            var boxMax = new Vector3(centroid.X, centroid.Y, Max.Z);
            var faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces, this));
            ChildNodes[0].DivideIntoBoxes();
            //bottomFrontRight
            boxMin = new Vector3(centroid.X, Min.Y, centroid.Z);
            boxMax = new Vector3(Max.X, centroid.Y, Max.Z);
            faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[1].DivideIntoBoxes();
            //topFrontLeft
            boxMin = new Vector3(Min.X, centroid.Y, centroid.Z);
            boxMax = new Vector3(centroid.X, Max.Y, Max.Z);
            faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[2].DivideIntoBoxes();
            //topFrontRight
            boxMin = centroid;
            boxMax = Max;
            faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[3].DivideIntoBoxes();
            //bottomBackLeft
            boxMin = Min;
            boxMax = centroid;
            faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[4].DivideIntoBoxes();
            //bottomBackRight
            boxMin = new Vector3(centroid.X, Min.Y, Min.Z);
            boxMax = new Vector3(Max.X, centroid.Y, centroid.Z);
            faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[5].DivideIntoBoxes();
            //topBackLeft
            boxMin = new Vector3(Min.X, centroid.Y, Min.Z);
            boxMax = new Vector3(centroid.X, Max.Y, centroid.Z);
            faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[6].DivideIntoBoxes();
            //topBackRight
            boxMin = new Vector3(centroid.X, centroid.Y, Min.Z);
            boxMax = new Vector3(Max.X, Max.Y, centroid.Z);
            faces = FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[7].DivideIntoBoxes();
        }

        private Vector3 GetCentroid()
        {
            return new Vector3((Max.X + Min.X) / 2, (Max.Y + Min.Y) / 2, (Max.Z + Min.Z) / 2);
        }

        public bool IsLeaf()
        {
            return ChildNodes == null;
        }

        public List<Triangle> FindTrianglesInBox(Vector3 pMin, Vector3 pMax, ref List<Triangle> faces)
        {
            var faceInBox = new List<Triangle>();
            var faceNotInBox = new List<Triangle>();
            foreach (var face in faces)
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

            faces.Clear();
            faces.AddRange(faceNotInBox);
            return faceInBox;
        }

    }
}