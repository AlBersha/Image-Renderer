using System.Collections.Generic;
using ConverterBase;
using Raytracer.Optimisation;
using Raytracer.Scene.Interfaces;

namespace Raytracer.Tracing
{
    public interface ITracer
    {

        public List<List<Pixel>> Trace(ISceneCreator sceneCreator, ITreeProvider octree);
    }
}