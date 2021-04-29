using Microsoft.Extensions.DependencyInjection;

namespace Raytracer.Optimisation
{
    public static class TreeDIExtension
    {
        public static void RegisterTree(this IServiceCollection collection)
        {
            collection.AddScoped<INode, OctreeNode>();
            collection.AddScoped<ITreeProvider, Octree>();
        }
    }
}