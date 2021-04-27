using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer.Optimisation
{
    public class Octree: ITreeProvider
    {
        public OctreeNode Root { get; private set; }
        
        public Octree()
        {
            Root = new OctreeNode(Vector3.Zero, Vector3.One, new List<Triangle>(), null);
        }

        public void CreateTree(Vector3 pMin, Vector3 pMax, List<Triangle> faces)
        {
            Root = new OctreeNode(pMin, pMax, faces, null);
          
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Root.DivideIntoBoxes();
            watch.Stop();
            var time = watch.ElapsedMilliseconds / 1000;
            
            // Console.WriteLine($"Octree build time: {time} s");
        }
    }
}