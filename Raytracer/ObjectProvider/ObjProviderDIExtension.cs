using Microsoft.Extensions.DependencyInjection;

namespace Raytracer.ObjectProvider
{
    public static class ObjProviderDIExtension
    {
        public static void RegisterObjectProvider(this IServiceCollection collection)
        {
            collection.AddScoped<IObjectFromFileProvider, CustomSceneObjectProvider>();
        }
    }
}