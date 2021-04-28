using Microsoft.Extensions.DependencyInjection;
using Raytracer.Scene.Interfaces;

namespace Raytracer.Scene
{
    public static class SceneDIExtension
    {
        public static void RegisterScene(this IServiceCollection collection)
        {
            collection.AddScoped<IScreenProvider, ScreenProvider>();
            collection.AddScoped<ISceneCreator, SceneCreator>();
            collection.AddScoped<IParamsProvider, ParamsProvider>();
        }
    }
}