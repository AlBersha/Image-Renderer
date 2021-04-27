using Microsoft.Extensions.DependencyInjection;

namespace Raytracer.Scene
{
    public static class SceneDIExtension
    {
        public static void RegisterScene(this IServiceCollection collection)
        {
            collection.AddScoped<IScreenProvider, ScreenProvider>();
            collection.AddScoped<ILightProvider, LightProvider>();
            collection.AddScoped<ICameraProvider, StaticCameraProvider>();
        }
    }
}