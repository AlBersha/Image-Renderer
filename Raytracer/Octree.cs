using System;
using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer
{
    public class OctreeNode
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

        public void Divide()
        {
            if (Faces.Count < 10)
                return;
            
            ChildNodes = new List<OctreeNode>(8);
            var centroid = GetCentroid();

            //bottomFrontLeft
            var boxMin = new Vector3(Min.X, Min.Y, centroid.Z);
            var boxMax = new Vector3(centroid.X, centroid.Y, Max.Z);
            var faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces, this));
            ChildNodes[0].Divide();
            //bottomFrontRight
            boxMin = new Vector3(centroid.X, Min.Y, centroid.Z);
            boxMax = new Vector3(Max.X, centroid.Y, Max.Z);
            faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[1].Divide();
            //topFrontLeft
            boxMin = new Vector3(Min.X, centroid.Y, centroid.Z);
            boxMax = new Vector3(centroid.X, Max.Y, Max.Z);
            faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[2].Divide();
            //topFrontRight
            boxMin = centroid;
            boxMax = Max;
            faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[3].Divide();
            //bottomBackLeft
            boxMin = Min;
            boxMax = centroid;
            faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[4].Divide();
            //bottomBackRight
            boxMin = new Vector3(centroid.X, Min.Y, Min.Z);
            boxMax = new Vector3(Max.X, centroid.Y, centroid.Z);
            faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[5].Divide();
            //topBackLeft
            boxMin = new Vector3(Min.X, centroid.Y, Min.Z);
            boxMax = new Vector3(centroid.X, Max.Y, centroid.Z);
            faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[6].Divide();
            //topBackRight
            boxMin = new Vector3(centroid.X, centroid.Y, Min.Z);
            boxMax = new Vector3(Max.X, Max.Y, centroid.Z);
            faces = Extension.FindTrianglesInBox(boxMin, boxMax, ref Faces);
            ChildNodes.Add(new OctreeNode(boxMin, boxMax, faces,this));
            ChildNodes[7].Divide();
        }

        private Vector3 GetCentroid()
        {
            return new Vector3((Max.X + Min.X) / 2, (Max.Y + Min.Y) / 2, (Max.Z + Min.Z) / 2);
        }

        public bool IsLeaf()
        {
            return ChildNodes == null;
        }
    }
    
    public class Octree
    {
        public readonly OctreeNode Root;
        public Octree(Vector3 pMin, Vector3 pMax, List<Triangle> faces)
        {
            Root = new OctreeNode(pMin, pMax, faces, null);
          
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Root.Divide();
            watch.Stop();
            var time = watch.ElapsedMilliseconds / 1000;
            
            Console.WriteLine($"Octree build time: {time} s");
        }
    }
}