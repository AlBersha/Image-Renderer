using Raytracer.ObjectProvider;

namespace Raytracer.Optimisation
{
    public interface ITreeProvider
    {
        public INode Root { get; set; }
        public INode CreateTree(ObjectModel objectModel);
    }
}