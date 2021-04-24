using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;

namespace Raytracer
{
    public class OctreeNode
    {
        public List<OctreeNode> childNodes;
        public OctreeNode Parent;
        public List<Face> Faces;

        public Vector3 pMin;
        public Vector3 pMax;
        public OctreeNode(Vector3 pMin, Vector3 pMax, IList<Face> faces, OctreeNode parent)
        {
            this.pMin = pMin;
            this.pMax = pMax;
            Faces = new List<Face>(faces);

            Parent = parent;
            childNodes = null;
        }

        public void Divide(IList<Vertex> vertices)
        {
            if (Faces.Count < 10)
                return;
            
            childNodes = new List<OctreeNode>(8);
            var centroid = GetCentroid();

            //bottomFrontLeft
            var p_Min = new Vector3(pMin.X, pMin.Y, centroid.Z);
            var p_Max = new Vector3(centroid.X, centroid.Y, pMax.Z);
            var faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces, this));
            childNodes[0].Divide(vertices);
            //bottomFrontRight
            p_Min = new Vector3(centroid.X, pMin.Y, centroid.Z);
            p_Max = new Vector3(pMax.X, centroid.Y, pMax.Z);
            faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces,this));
            childNodes[1].Divide(vertices);
            //topFrontLeft
            p_Min = new Vector3(pMin.X, centroid.Y, centroid.Z);
            p_Max = new Vector3(centroid.X, pMax.Y, pMax.Z);
            faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces,this));
            childNodes[2].Divide(vertices);
            //topFrontRight
            p_Min = centroid;
            p_Max = pMax;
            faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces,this));
            childNodes[3].Divide(vertices);
            //bottomBackLeft
            p_Min = pMin;
            p_Max = centroid;
            faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces,this));
            childNodes[4].Divide(vertices);
            //bottomBackRight
            p_Min = new Vector3(centroid.X, pMin.Y, pMin.Z);
            p_Max = new Vector3(pMax.X, centroid.Y, centroid.Z);
            faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces,this));
            childNodes[5].Divide(vertices);
            //topBackLeft
            p_Min = new Vector3(pMin.X, centroid.Y, pMin.Z);
            p_Max =new Vector3(centroid.X, pMax.Y, centroid.Z);
            faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces,this));
            childNodes[6].Divide(vertices);
            //topBackRight
            p_Min = new Vector3(centroid.X, centroid.Y, pMin.Z);
            p_Max =new Vector3(pMax.X, pMax.Y, centroid.Z);
            faces = Extension.FindTrianglesInBox(p_Min, p_Max, ref Faces, vertices);
            childNodes.Add(new OctreeNode(p_Min, p_Max, faces,this));
            childNodes[7].Divide(vertices);
        }

        private Vector3 GetCentroid()
        {
            return new Vector3((pMax.X + pMin.X) / 2, (pMax.Y + pMin.Y) / 2, (pMax.Z + pMin.Z) / 2);
        }

        public bool IsLeaf()
        {
            return childNodes == null;
        }

        public List<Face> GetParentsFaces()
        {
            var list = new List<Face>();
            if (Parent == null)
                return Faces;
            
            list.AddRange(Faces);
            list.AddRange(Parent.GetParentsFaces());
            return list;
        }
    }
    
    public class Octree
    {
        public OctreeNode Root;
        public Octree(Vector3 pMin, Vector3 pMax, IList<Face> faces, IList<Vertex> vertices)
        {
            Root = new OctreeNode(pMin, pMax, faces, null);
            Root.Divide(vertices);
        }

        public void Traverse()
        {
            
        }
    }
}