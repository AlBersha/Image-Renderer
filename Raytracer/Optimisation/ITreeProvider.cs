using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;

namespace Raytracer.Optimisation
{
    public interface ITreeProvider
    {
        public void CreateTree(Vector3 pMin, Vector3 pMax, List<Triangle> faces);
    }
}