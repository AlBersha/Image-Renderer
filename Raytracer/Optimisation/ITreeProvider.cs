using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;
using Raytracer.ObjectProvider;

namespace Raytracer.Optimisation
{
    public interface ITreeProvider
    {
        public void CreateTree(ObjectModel objectModel);
    }
}