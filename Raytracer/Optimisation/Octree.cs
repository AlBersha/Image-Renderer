using System;
using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;
using Raytracer.ObjectProvider;

namespace Raytracer.Optimisation
{
    public class Octree: ITreeProvider
    {
        public OctreeNode Root { get; private set; }
        
        public Octree()
        {
            Root = new OctreeNode(Vector3.Zero, Vector3.One, new List<Triangle>(), null);
        }

        public void CreateTree(ObjectModel object3D)
        {
            Vector3 pMin = Vector3.Zero;
            Vector3 pMax = Vector3.Zero;
            BoundingBoxCoordinates(object3D, ref pMin, ref pMax);
            Root = new OctreeNode(pMin, pMax, object3D.Faces, null);
          
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Root.DivideIntoBoxes();
            watch.Stop();
            var time = watch.ElapsedMilliseconds / 1000;
            
            // Console.WriteLine($"Octree build time: {time} s");
        }
        
        private void BoundingBoxCoordinates(ObjectModel object3D, ref Vector3 pMin, ref Vector3 pMax)
        {
            var xMin = float.MaxValue;
            var yMin = float.MaxValue;
            var zMin = float.MaxValue;
            var xMax = float.MinValue;
            var yMax = float.MinValue;
            var zMax = float.MinValue;
            
            foreach (var t in object3D.Vertices)
            {
                xMin = Math.Min(xMin, t.X);
                yMin = Math.Min(yMin, t.Y);
                zMin = Math.Min(zMin, t.Z);
                xMax = Math.Max(xMax, t.X);
                yMax = Math.Max(yMax, t.Y);
                zMax = Math.Max(zMax, t.Z);
            }

            pMin = new Vector3(xMin, yMin, zMin);
            pMax = new Vector3(xMax, yMax, zMax);
        }
    }
}